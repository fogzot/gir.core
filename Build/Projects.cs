using System;
using System.Collections.Generic;
using System.Linq;

namespace Build
{
    // TODO: Instead of declaring the folder here, maybe we should use LoadedProject?
    // Alternatively, we might want to choose where certain projects are generated,
    // e.g. to put GLib and GObject in a separate dedicated folder.
    public record Project(string GirFile, string Folder)
    {
        public static Project FromName(string name)
        {
            return new Project($"{Projects.GirPath}/{name}.gir", $"{Projects.ProjectPath}/{name}");
        }
    }
    
    public static class Projects
    {
        #region Constants
        
        public const string ProjectPath = "../Libs";
        public const string GirPath = "../gir-files";
        
        public const string SOLUTION = "../GirCore.sln";

        private const string SAMPLE = "../Samples/";
        private const string DBUS_SAMPLE = SAMPLE + "DBus/";
        private const string GSTREAMER_SAMPLE = SAMPLE + "GStreamer/";
        private const string GTK3_APP_SAMPLE = SAMPLE + "Gtk3/GtkApp/";
        private const string GTK3_BUILDER_SAMPLE = SAMPLE + "Gtk3/Builder";
        private const string GTK3_QUICKSTART = SAMPLE + "Gtk3/QuickStart";
        private const string GTK3_COMPOSITE_TEMPLATE_SOURCEGENERATOR = SAMPLE + "Gtk3/CompositeTemplates/UsingSourceGenerator";
        private const string GTK3_COMPOSITE_TEMPLATE_NO_SOURCEGENERATOR = SAMPLE + "Gtk3/CompositeTemplates/NoSourceGenerator";
        private const string GTK4_SIMPLE_WINDOW_SAMPLE = SAMPLE + "Gtk4/SimpleWindow/";
        private const string GDKPIXBUF_TEST_MEMORY_LEAKS = SAMPLE + "GdkPixbuf/TestMemoryLeaks";

        private const string INTEGRATION = "../Integration/";
        
        #endregion
        
        #region Fields

        public static readonly string[] IntegrationProjects =
        {
            INTEGRATION
        };

        public static readonly string[] SampleProjects =
        {
            DBUS_SAMPLE, 
            GSTREAMER_SAMPLE,
            //GTK3_APP_SAMPLE,
            GTK3_BUILDER_SAMPLE, 
            GTK3_QUICKSTART,
            GTK3_COMPOSITE_TEMPLATE_SOURCEGENERATOR,
            GTK3_COMPOSITE_TEMPLATE_NO_SOURCEGENERATOR,
            //GTK4_SIMPLE_WINDOW_SAMPLE
            GDKPIXBUF_TEST_MEMORY_LEAKS
        };

        public static IEnumerable<Project> AllLibraries = new[]
        {
            Project.FromName("GLib-2.0"),
            Project.FromName("GObject-2.0"),
            Project.FromName("Gio-2.0"),
            Project.FromName("cairo-1.0"),
            Project.FromName("Pango-1.0"),
            Project.FromName("Gdk-3.0"),
            Project.FromName("GdkPixbuf-2.0"),
            Project.FromName("Atk-1.0"),
            Project.FromName("Gtk-3.0"),
            Project.FromName("Gst-1.0"),
            Project.FromName("GstAudio-1.0"),
            Project.FromName("GstVideo-1.0"),
            Project.FromName("GstPbutils-1.0"),
            Project.FromName("GstBase-1.0")
        };

        #endregion
    }
}
