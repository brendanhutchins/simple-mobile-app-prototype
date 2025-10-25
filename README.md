# Mobile App Prototype
I was the sole Unity/C# developer on this project. I implemented the full UI/UX flow, captions display, and data saving capabilities.

Unity version: 2021.3
Platform: Android (mobile)

Main Scripts: 
- ScreenChanger.cs controls all pages in the app, handling unity events, button interactions, and switching screens. It is a modular design that can handle any number of screens being added or removed.
- AudioFiles.cs and PlaybackControls.cs feature an AV playback system with synchronized captions set by the ScreenData and CaptionsData scriptable objects (found in the Data folder in _Scripts).

Skills Demonstrated:
- UI state management
- Screen layout and interaction helpers: found in _Scripts/UI
- Analytics: full application sends collected data to a Firebase server, with time tracking and questionnaire result data being sent
- Media synchronization and playback: videos start with a delay, so audio needs to wait for video to load fully to begin playing.
- Localization: application is set to be in Spanish by default but can be set to English for debugging purposes

Video demonstration of full application: https://youtu.be/sQGYErl1C9Q
