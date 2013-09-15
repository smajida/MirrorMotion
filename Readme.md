## MirrorMotion

MirrorMotion is a proof-of-concept library that runs actions based on user's hand gestures tracked by Microsoft Kinect.

Solution contains Core library and demo clients (Console/Windows Service/System Tray App).

Currently allowed actions are System.Action delegates, plugin system is not implemented yet.

#### Example

MirrorMotion.TrayApp project is an application that stays in system tray and closes current foreground window when user makes a horizontal right swipe with closed hand.