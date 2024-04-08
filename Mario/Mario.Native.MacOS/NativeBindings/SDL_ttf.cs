using System;
using System.Runtime.InteropServices;

namespace Mario.Native.MacOS.NativeBindings
{
    public static class SDL_ttf
    {
        static SDL_ttf()
        {
            var libraryPath = Environment.GetEnvironmentVariable("SDL2_TTF_PATH");
            if (libraryPath == null)
                throw new InvalidOperationException(
                    "The SDL2_TTF_PATH environment variable is not set."
                );

            IntPtr libHandle = NativeLibrary.Load(libraryPath);

            TTF_Init = Marshal.GetDelegateForFunctionPointer<TTF_InitDelegate>(
                NativeLibrary.GetExport(libHandle, "TTF_Init")
            );
            TTF_Quit = Marshal.GetDelegateForFunctionPointer<TTF_QuitDelegate>(
                NativeLibrary.GetExport(libHandle, "TTF_Quit")
            );
            TTF_OpenFont = Marshal.GetDelegateForFunctionPointer<TTF_OpenFontDelegate>(
                NativeLibrary.GetExport(libHandle, "TTF_OpenFont")
            );
            TTF_RenderText_Solid =
                Marshal.GetDelegateForFunctionPointer<TTF_RenderText_SolidDelegate>(
                    NativeLibrary.GetExport(libHandle, "TTF_RenderText_Solid")
                );
            TTF_CloseFont = Marshal.GetDelegateForFunctionPointer<TTF_CloseFontDelegate>(
                NativeLibrary.GetExport(libHandle, "TTF_CloseFont")
            );
        }

        public delegate int TTF_InitDelegate();
        public delegate void TTF_QuitDelegate();
        public delegate IntPtr TTF_OpenFontDelegate(string file, int ptsize);
        public delegate IntPtr TTF_RenderText_SolidDelegate(
            IntPtr font,
            string text,
            SDL_Color color
        );
        public delegate void TTF_CloseFontDelegate(IntPtr font);

        public static TTF_InitDelegate TTF_Init;
        public static TTF_QuitDelegate TTF_Quit;
        public static TTF_OpenFontDelegate TTF_OpenFont;
        public static TTF_RenderText_SolidDelegate TTF_RenderText_Solid;
        public static TTF_CloseFontDelegate TTF_CloseFont;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_Color
    {
        public byte r,
            g,
            b,
            a;
    }
}
