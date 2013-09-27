---
layout: post
title: Understanding HtmlOutput
---

SocialToolBox uses a rather unusual pattern for rendering pages, a
good example being the `HtmlOutput` class. 

At a glance, `HtmlOutput` looks fairly similar to a `StringBuilder`:
create one, call its `Add()` functions to append HTML content, and use
`Build()` to receive a fresh copy of the resulting HTML.

There is, however, a critical difference ; the reason lies in the way
the `StringBuilder` usage pattern interacts with asynchronous tasks.
Consider the following example, where we render two objects that are
both queried from the database:

{% highlight c# %}
async Task Render(string query, StringBuilder builder)
{
  var result = await Execute(query);
  builder.AppendFormat("{0}:{1}", query, result);
}

Task BadOrder(StringBuilder builder)
{
  var a = Render("Query A", builder);
  var b = Render("Query B", builder);
  return Task.WhenAll(a, b);
}

async Task NotAsync(StringBuilder builder)
{
  await Render("Query A", builder);
  await Render("Query B", builder);
}
{% endhighlight %}

In function `BadOrder`, both rendering functions are launched at the
same time, and the results will be rendered in the order they were
received. 

In function `NotAsync`, the two function calls are sequential: query B
will only be sent to the database after the result of query A has been
rendered.

There are, of course, ways to alter the signature of `Render()` so
that the ordering is guaranteed while retaining asynchronous
execution. For instance, by returning the rendered string instead of
appending it to a builder:

{% highlight c# %}
async Task<string> Render(string query)
{
  var result = await Execute(query);
  return query + ":" + result;
}

async Task<string> CorrectButSlow()
{
  var a = Render("Query A");
  var b = Render("Query B");
  return (await a) + (await b);
}
{% endhighlight %}

Function `CorrectButSlow` will start both queries independently, but
will wait until both results are available to concatenate them in the
correct order. Of course, it reverts to concatenating strings all over
the place, and loses all the performance benefits of using a
`StringBuilder` in the first place.

Other changes to `Render` are possible, such as splitting the function
into an asynchronous part which performs queries, and a synchronous
part which renders the result. Again, functional style with currying
helps solve the problem:

{% highlight c# %}
async Task<Action<StringBuilder>> Render(string query)
{
  var result = await Execute(query);
  return b => b.AppendFormat("{0}:{1}", query, result);
}

async Task<Action<StringBuilder>> CorrectButUgly()
{
  var a = Render("Query A");
  var b = Render("Query B");
  var a_render = await a;
  var b_render = await b;
  return builder => 
  {
    a_render(builder);
    b_render(builder);
  };
}
{% endhighlight %}

Function `CorrectButUgly` achieves maximum performance by using both
`StringBuilder` to avoid quadratic concatenation, and supporting
asynchronous queries with correct ordering. However, this ends up
generating awkward code: the function return types are overly complex, 
and all the synchronous rendering is moved to a lambda at the end of 
the function, breaking control flow.

A more elegant solution is to make the builder object async-aware. A
first step would be adding a function like `AppendAsync(Task<string>
t)` which schedules the result of task `t` for addition at that
specific point in the builder without actually waiting for the task to
finish.

{% highlight c# %}
void Render(string query, AsyncStringBuilder builder)
{
  var resultAsync = Execute(query);
  builder.AppendFormat("{0}:", query);
  builder.AppendAsync(resultAsync);
}

void Correct(AsyncStringBuilder builder)
{
  Render("Query A", builder);
  Render("Query B", builder);
}
{% endhighlight %}

With this pattern, the order in which the functions are called will
determine the order in which the results will appear in the builder,
but processing will remain asynchronous. Even better, the asynchronous
aspects have all been delegated to the builder itself: the
responsibility of the main code is to *determine what should be
rendered*, and the builder is responsible for turning the resulting
asynchronous soup of tasks into the correct sequence.

In terms of implementation, this means that the asynchronous builder
retains a queue of all requested append operations, and will simply
run through them all when its `ToString()` function is called. 

However, this is not good enough. Let's see what happens when
conditional rendering happens:

{% highlight c# %}
async Task Render(string query, AsyncStringBuilder builder)
{
  var allowed = await IsAllowed(query);
  if (allowed) 
  {
    builder.AppendFormat("{0}:", query);
    builder.AppendAsync(Execute(query));
  }
  else 
  {
    builder.AppendFormat("Query {0} is not allowed");
  }
}

Task Incorrect(AsyncStringBuilder builder)
{
  var a = Render("Query A", builder);
  var b = Render("Query B", builder);
  return Task.WhenAll(a,b);
}
{% endhighlight %}

In this example, the calls to `AppendAsync()` will happen in the order in
which the two calls to `IsAllowed` return their results, which might
not be the expected order.

There are many situations where several append calls are dependent on
a single asynchronous value. Even with `AppendAsync()`, there is no
simple way to turn all these append calls into a single `Task<string>`
(at least, not without resorting to string concatenation again).

The solution is to let the builder object actually expose the
*checkpoints*. In other words, before starting an asynchronous
section, you could ask the builder for a sub-builder that inserts
whatever it builds at the current location, even if other things are
appended to the builder in the mean time.

An example API (which is not what `HtmlOutput`uses): 

{% highlight c# %}

var builder = new AsyncStringBuilder();
builder.Append("A");

// Contents are 'A'

var checkpoint = builder.GetCheckpoint();
builder.Append("B");

// Contents are 'AB'

checkpoint.Append("C");

// Contents are 'ACB'

{% endhighlight %}

Using this pattern, it's fairly easy to rewrite the `Incorrect` function
like this: 

{% highlight c# %}
Task Correct(AsyncStringBuilder builder)
{
  var a = Render("Query A", builder.GetCheckpoint());
  var b = Render("Query B", builder.GetCheckpoint());
  return Task.WhenAll(a,b);
}
{% endhighlight %}

However, this is fairly error-prone: if you forget the call to
`GetCheckpoint` (a likely mistake), ordering becomes
non-deterministic.

To avoid this, the actual `HtmlOutput` uses a callback-like API which
wraps the asynchronous operation in a function call:

{% highlight c# %}
void Correct(HtmlOutput output)
{
  output.Insert(o => Render("Query A", o));
  output.Insert(o => Render("Query B", o));
}
{% endhighlight %}

With this pattern, `Render("Query A", output)` would issue a warning,
because it returns a task that is neither assigned nor awaited
on. `HtmlOutput` actually takes responsibility for the task returned
by `Render()`, so that the `Correct()` function is not explicitly
asynchronous anymore.
