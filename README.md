# Introduction
:::::This readme is under construction:::::

Helibot is an automatic moderation bot that will Chat Ban users who use certain banned phrases/words that are configurable by the server owner.
This is my first sizable c# project so things are bound to be buggy and/or broken and/or unoptimised.

# Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
1.	Installation process:
    As of right now, Helibot must be run from the Visual Studio debugger due to the limitations of the Discord.Net API library, which requires the project be a .NET Core rather than Framework
    Helibot will be migrated to Core as soon as possible. Keep an eye on the Changelog and for future commits to this repository
2.	Software dependencies
    Helibot must be run from the Visual Studio debugger. (I know, i'm working on that)
    Requires .NET Framework 1.7.1
3.	Latest releases
    Version 1.0 was released on 7/11/2018::21:15 PST
    Current Features:
    - Manual and automatic chat banning
    - Various developer commands
    Check the Planned Features section of this README for features planned for the future.
4.	API references
    Requires Discord.Net version 1.0.2
# Build and Test
    It's easy. Click run. I have not included any unit tests.

# Contribute
    Contributions are not availiable at this time, however I plan on accepting them in the VERY near future. Stay tuned.

#Planned Features
    - Add Banned word command ("//AddWord")
    - Add Remove word command ("//DelWord")
    - Add timed automatic unbanning for bans instantiated by the Automatic banning system
    - Add a packed GUI for changing things such as banned words and BotTokens.
