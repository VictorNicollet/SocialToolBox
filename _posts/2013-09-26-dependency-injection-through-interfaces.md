---
layout: post
title: Dependency injection through interfaces
---

SocialToolBox uses dependency injection to remain platform-agnostic:
one can easily add support for more database servers renderers or web
servers, simply by implementing the appropriate interface.

Interface `SocialToolBox.Core.Database.IDatabaseDriver` deals with
database persistence, which covers two distinct concepts: event
streams and projections. Both can be handled independently, though
default implementations will save them to the same SQL database.

Interface `SocialToolBox.Core.Web.IWebDriver` deals with the web side:
how requests are received and responses are sent. A default
implementation, for running your social applications as an IIS
`HttpHandler`, is provided in project `SocialToolBox.Core.Web.IIS`.

Rendering is a bit different. Instead of directly calling the member
functions of an interface, pages will build a tree of page nodes (that
implement `SocialToolBox.Core.Present.IPageNode`). This tree is then
provided to a visitor that turns each page node into the corresponding
HTML. This allows for multiple rendering strategies based on different
CSS frameworks, or depending on whether the client is a mobile or
desktop browser.

All of these interfaces support mocks, which lets us unit test every
single nook and cranny of the toolbox.
