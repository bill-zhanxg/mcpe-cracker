# Gaming Services DLL Setup Instructions

## Background

As of recent versions, Minecraft Bedrock Edition uses the Xbox Gaming Services SDK for license validation instead of the Windows Store API. This requires replacing the `GamingServices.dll` file in addition to (or instead of) the `Windows.ApplicationModel.Store.dll`.

## Adding Gaming Services DLL Resources

To enable the new SDK crack method, follow these steps:

### 1. Obtain Cracked DLL Files

You need to obtain cracked versions of `GamingServices.dll` for both x64 and x86 architectures:
- `GamingServices.dll` (x64 version)
- `GamingServices x86.dll` (x86 version)

### 2. Add Files to Resources Folder

1. Copy the cracked DLL files to: `MCPECracker/Resources/`
   - Place x64 version as: `GamingServices.dll`
   - Place x86 version as: `GamingServices x86.dll`

### 3. Update Resource File

1. Open the project in Visual Studio
2. Navigate to `Properties/Resources.resx`
3. Add the new DLL files:
   - Click "Add Resource" → "Add Existing File"
   - Select `GamingServices.dll` from the Resources folder
   - Repeat for `GamingServices x86.dll`
4. Note the resource names (should be `GamingServices` and `GamingServices_x86`)

### 4. Update Code References

Open `MCPECracker/Forms/Form1.cs` and locate the `Form1_Load` method. Update the Gaming Services DLL processing call:

```csharp
// Process Gaming Services DLL (new SDK method)
if (!ProcessDllFile(gamingServicesPath, 
    Properties.Resources.GamingServices,      // Remove null, use actual resource
    Properties.Resources.GamingServices_x86,  // Remove null, use actual resource
    ref progressValue))
{
    return;
}
```

Change from:
```csharp
null, // Properties.Resources.GamingServices when available
null, // Properties.Resources.GamingServices_x86 when available
```

To:
```csharp
Properties.Resources.GamingServices,
Properties.Resources.GamingServices_x86,
```

### 5. Rebuild the Project

1. Build the solution in Release mode
2. Test the executable on a Windows system with Minecraft installed

## Target File Location

The tool will target: `C:\Windows\System32\GamingServices.dll`

## Important Notes

- **Administrator privileges are required** - The tool automatically requests elevation
- **Backup recommended** - The revert function uses System File Checker, but manual backup is advised
- **Minecraft must be closed** - The tool will attempt to kill processes locking the DLL
- **Test carefully** - This modification affects system files and should be thoroughly tested

## Troubleshooting

### Issue: GamingServices.dll not found
- Verify the file exists in `C:\Windows\System32\`
- On older Windows versions, Gaming Services may not be installed

### Issue: File is in use
- Close Minecraft and all Xbox/Gaming Services processes
- Close Xbox app and Gaming Services in Task Manager
- Reboot if necessary

### Issue: Access denied
- Ensure the tool is running as Administrator
- Check Windows Defender / Antivirus isn't blocking the operation

## Alternative Approach

If `GamingServices.dll` alone doesn't work, you may need to target additional files:
- `Microsoft.Gaming.Services.dll`
- Files in `C:\Program Files\WindowsApps\Microsoft.GamingServices_*`

The code can be extended to handle additional DLLs by adding more `ProcessDllFile` calls in the `Form1_Load` method.
