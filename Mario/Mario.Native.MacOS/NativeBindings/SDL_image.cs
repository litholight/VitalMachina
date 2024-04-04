using System;
using System.Runtime.InteropServices;

namespace Mario.Native.MacOS.NativeBindings
{
    public static class SDL_image
    {
        private const string SDLImageLibName = "SDL2_image";

        [DllImport(SDLImageLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int IMG_Init(IMG_InitFlags flags);

        [DllImport(SDLImageLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void IMG_Quit();

        [DllImport(SDLImageLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr IMG_LoadTexture(IntPtr renderer, string file);

        // Additional SDL2_image functions as needed
        [Flags]
        public enum IMG_InitFlags
        {
            IMG_INIT_JPG = 0x00000001,
            IMG_INIT_PNG = 0x00000002,
            // Add other flags as necessary
        }
    }
}
