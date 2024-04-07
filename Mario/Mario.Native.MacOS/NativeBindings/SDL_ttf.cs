using System;
using System.Runtime.InteropServices;

namespace Mario.Native.MacOS.NativeBindings
{
    public static class SDL_ttf
    {
        private const string SDLTTFNativeLib = "/usr/local/opt/sdl2_ttf/lib/libSDL2_ttf.dylib";

        [DllImport(SDLTTFNativeLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_Init();

        [DllImport(SDLTTFNativeLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TTF_Quit();

        [DllImport(SDLTTFNativeLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_OpenFont(string file, int ptsize);

        [DllImport(SDLTTFNativeLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderText_Solid(IntPtr font, string text, SDL_Color color);

        [DllImport(SDLTTFNativeLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TTF_CloseFont(IntPtr font);

        // Add additional SDL2_ttf function bindings here as needed
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_Color
    {
        public byte r, g, b, a;
    }
}
