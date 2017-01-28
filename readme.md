Zen.Iot

Zen.Aero.Media.Client

This UWP application is designed to provide a transit space that can be the target for media file uploads from your various devices (phone, camera etc)
The app handles thumbnail generation for easier browsing and manages upload to the cloud using background file transfer.
In addition the app will use local storage as a cache for pulling down images from the cloud as necessary.

The local drivespace must be split into two sections
* Thumbnail and tag mapping
* Gallery image cache

The gallery image cache must be able to flag an image in such a way as to be able to track the following;
* Source device
* Remote cloud location
* Upload timestamp
* Last access timestamp

Scavenging
When space allocated to the gallery cache passes the max threshold, the scavenging process must be able to remove files based on LRU
semantics until an appropriate amount of space has been reclaimed.
It will not be possible to scavenge files that have yet to be uploaded to the cloud.

Tagging
Using a self-organising map, similar images will be used to generate tagging suggestions

Screen-saver/slideshow
When the media application is run on a device with a display, a slideshow of recently uploaded images will be available