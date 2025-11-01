# MCPE Cracker
> A small tool for automatic cracking `Minecraft for Windows Edition`


**I do not suggestion play on a cracked Minecraft, use this at your own risk!**

This program is written in C# WinForm. Previously, it replaced the `Windows.ApplicationModel.Store.dll` in `C:\Windows\System32` with a cracked one. However, **Minecraft Bedrock Edition has switched to using Gaming Services SDK** for licensing, which requires additional steps.

## Recent Changes (SDK Update)

**Important Notice**: As of recent versions, Minecraft Bedrock Edition uses the Xbox Gaming Services SDK instead of the Windows Store API for license validation. This tool now supports both methods:

1. **Legacy Method**: Replaces `Windows.ApplicationModel.Store.dll` (for older versions)
2. **New SDK Method**: Targets `GamingServices.dll` (for current versions)

To use this tool with the new SDK method, you may need to obtain the appropriate cracked `GamingServices.dll` files (x64 and x86 versions) and add them to the `Resources` folder.

**This program does not work for `Minecraft Java Edition`**. I personally don't know how to crack it, but all you need to have is Xbox Game Pass to be able to play `Minecraft Java Edition`!

<br />

This program is thoroughly tested and developed on `Windows 11 Home Edition x64`, and it should work on **any Windows 11/10 Edition with x64 OS**.

<br />

Untried platforms:
* Windows 11 x86
* Windows 10 x86
* Any Windows OS below Windows 10
