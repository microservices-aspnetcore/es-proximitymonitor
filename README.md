# Proximity Monitor
The proximity monitor application is listening for **MemberProximityDetected** events. This is just to illustrate how to create a monitor for an event stream emitted by your event processor. As you'll read in the book, you can have multiple event processors, multiple outbound streams, and multiple monitors. How you design it depends largely on your expected volume and the domain of your application.

In the case of the simple sample for the book, it's just a little page that displays a notification whenever a member's location is reported near another member. You can imagine more visually appealing user interfaces for such a monitor, including a map integration that drops animated "pings" onto a map to illustrate nearby members. You could integrate such a map with the _reality_ service to show everyone's current location and to highlight the proximity events.

