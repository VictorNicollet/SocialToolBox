--- 
layout: post
title: Event Streaming
---

The recommended storage layer for SocialToolBox is an SQL database,
but the data model is not relational: instead, it uses *Event Sourcing*.

In a traditional web application, any user actions are applied to the
database straight away. New user signs up ? A new line is inserted
into the user table. Article title changed ? The corresponding line is
updated in the article table.

Event Sourcing introduces an event queue between the user and the
database. Now, when changing the title of an article, the application
would enqueue a matching `ArticleTitleChanged` event, and a
background process would read that event and apply it to the database.

This approach is far more robust against usage spikes: unlike complex
transactional updates, appending an event to a table is a very fast
operation. Sure, the database may lag behind the event queue (because
events cannot be processed fast enough) but this is still better than
randomly dropping clients.

However, the main benefit is the support for schema migration. In a
typical relational setup, changing the database schema involves a soup
of `ALTER`, `INSERT INTO t SELECT`, table swapping and asynchronous
data migration. This is already a difficult enough process when your
ops team is in-house, but it becomes lethal for open source projects.
And, unless you're doing some black magic along the way, the database
is unavailable while migration is underway.

The event queue is preserved forever. If the database schema changes,
all you have to do is deploy the new application code, create a new
database and re-run the entire event queue. When the new database
catches up with the event stream, deploy the application to production
and drop the old database. There is no downtime, and there is no limit
on the magnitude of the changes you can perform. You can even go as
far as un-delete data which was erased from the database but was still
present in the event stream.

As an added benefit, it now becomes easy to split a database into
several independent pieces called "projections". Each projection may
run on a different server and can be migrated to a new schema
independently. The only requirement is that database queries can of
course only read from one projection at a time, but most applications
have a schema that lends itself well to such separation with only
minimal data duplication.