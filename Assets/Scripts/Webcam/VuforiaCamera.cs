using UnityEngine;
using Vuforia;
using Wizcorp.Utils.Logger;

/* 
Author: 
    Maximetinu 
    maximetinu@gmail.com 
    github.com/maximetinu
Copyright: uncopyright.
License: DO What The F*ck You Want To Public License

Class usage:
initialize Scanner (BarcodeScanner in the demo scripts, line 26) as follows:
    BarcodeScanner = new Scanner(null, null, new BarcodeScanner.Webcam.VuforiaCamera());
 */

namespace BarcodeScanner.Webcam
{
    public class VuforiaCamera : IWebcam
    {
        public Texture Texture { 
            get {
                vuforia2DTexture = new Texture2D(VuforiaARCamera.Width, VuforiaARCamera.Height);
                VuforiaARCamera.CopyToTexture(vuforia2DTexture);    
                return vuforia2DTexture; 
        }}
        private Texture2D vuforia2DTexture;
        public Image VuforiaARCamera { get; private set; }

        public int Width { get { return VuforiaARCamera.Width;}}
        public int Height { get { return VuforiaARCamera.Height;}}

        private Image.PIXEL_FORMAT currentPixelFormat;

        public VuforiaCamera()
        {
            // RGBA8888 is the most common pixel format, but it may not work with your device. Test others. 
            VuforiaARCamera = CameraDevice.Instance.GetCameraImage(Image.PIXEL_FORMAT.RGBA8888);
            currentPixelFormat = Image.PIXEL_FORMAT.RGBA8888;
        }

        public VuforiaCamera(Image.PIXEL_FORMAT pixelFormat)
        {
            VuforiaARCamera = CameraDevice.Instance.GetCameraImage(pixelFormat);
            currentPixelFormat = pixelFormat;
        }

        // Unnecessary operation for Vuforia camera
        public void SetSize(){}

        // If not ready, try again to initialize it. Vuforia takes time to get ready.
        public bool IsReady()
        {
            bool isReady = (VuforiaARCamera != null);
            if (!isReady)
                this.Play();
            return isReady;
        }

        public bool IsPlaying()
        {
            return CameraDevice.Instance.IsActive();
        }

        public void Play()
        {
            VuforiaARCamera = CameraDevice.Instance.GetCameraImage(currentPixelFormat);
        }

        // Manage Stop and Destroy from Vuforia's API.
        public void Stop(){}
        public void Destroy(){}

        public Color32[] GetPixels(Color32[] data = null)
        {
            vuforia2DTexture = new Texture2D(VuforiaARCamera.Width, VuforiaARCamera.Height);
            VuforiaARCamera.CopyToTexture(vuforia2DTexture);
            return this.vuforia2DTexture.GetPixels32();
        }

        public int GetChecksum()
        {
            return VuforiaARCamera.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[VuforiaCamera | Resolution: {0}x{1}", Width, Height);
        }
    }
}
