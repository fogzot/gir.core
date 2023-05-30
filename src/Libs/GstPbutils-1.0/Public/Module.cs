﻿namespace GstPbutils;

public class Module
{
    private static bool IsInitialized;

    public static void Initialize()
    {
        if (IsInitialized)
            return;

        GObject.Module.Initialize();
        Gst.Module.Initialize();
        GstBase.Module.Initialize();
        GstAudio.Module.Initialize();
        GstVideo.Module.Initialize();

        Internal.ImportResolver.RegisterAsDllImportResolver();
        Internal.TypeRegistration.RegisterTypes();

        IsInitialized = true;
    }
}
