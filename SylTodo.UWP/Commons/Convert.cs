﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace SylTodo.UWP.Commons {
    class Convert {
        public static async Task<BitmapImage> ConvertByteToImage(byte[] imageBytes) {
            if (imageBytes != null) {
                MemoryStream stream = new MemoryStream(imageBytes);
                var randomAccessStream = new MemoryRandomAccessStream(stream);
                BitmapImage bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(randomAccessStream);
                return bitmapImage;
            } else {
                return new BitmapImage(new Uri("ms-appx:///Assets/background.jpg")); // 默认图片
            }
        }

        public static async Task<byte[]> ConvertImageToByte(StorageFile file) {
            using (var inputStream = await file.OpenSequentialReadAsync()) {
                var readStream = inputStream.AsStreamForRead();
                var byteArray = new byte[readStream.Length];
                await readStream.ReadAsync(byteArray, 0, byteArray.Length);
                return byteArray;
            }
        }

        public static async Task<InMemoryRandomAccessStream> ConvertByteToRandomAccessStream(byte[] arr) {
            InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
            await randomAccessStream.WriteAsync(arr.AsBuffer());
            randomAccessStream.Seek(0); // Just to be sure.
                                        // I don't think you need to flush here, but if it doesn't work, give it a try.
            return randomAccessStream;
        }
    }


    class MemoryRandomAccessStream : IRandomAccessStream {
        private Stream m_InternalStream;

        public MemoryRandomAccessStream(Stream stream) {
            this.m_InternalStream = stream;
        }

        public MemoryRandomAccessStream(byte[] bytes) {
            this.m_InternalStream = new MemoryStream(bytes);
        }

        public IInputStream GetInputStreamAt(ulong position) {
            this.m_InternalStream.Seek((long)position, SeekOrigin.Begin);

            return this.m_InternalStream.AsInputStream();
        }

        public IOutputStream GetOutputStreamAt(ulong position) {
            this.m_InternalStream.Seek((long)position, SeekOrigin.Begin);

            return this.m_InternalStream.AsOutputStream();
        }

        public ulong Size {
            get { return (ulong)this.m_InternalStream.Length; }
            set { this.m_InternalStream.SetLength((long)value); }
        }

        public bool CanRead {
            get { return true; }
        }

        public bool CanWrite {
            get { return true; }
        }

        public IRandomAccessStream CloneStream() {
            throw new NotSupportedException();
        }

        public ulong Position {
            get { return (ulong)this.m_InternalStream.Position; }
        }

        public void Seek(ulong position) {
            this.m_InternalStream.Seek((long)position, 0);
        }

        public void Dispose() {
            this.m_InternalStream.Dispose();
        }

        public Windows.Foundation.IAsyncOperationWithProgress<IBuffer, uint> ReadAsync(IBuffer buffer, uint count, InputStreamOptions options) {
            var inputStream = this.GetInputStreamAt(0);
            return inputStream.ReadAsync(buffer, count, options);
        }

        public Windows.Foundation.IAsyncOperation<bool> FlushAsync() {
            var outputStream = this.GetOutputStreamAt(0);
            return outputStream.FlushAsync();
        }

        public Windows.Foundation.IAsyncOperationWithProgress<uint, uint> WriteAsync(IBuffer buffer) {
            var outputStream = this.GetOutputStreamAt(0);
            return outputStream.WriteAsync(buffer);
        }
    }
}
