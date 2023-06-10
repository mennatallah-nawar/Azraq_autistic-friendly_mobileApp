using UnityEngine;
using System.IO;

public static class SavWav
{
    public static string filePath = null;
    public static bool Save(string filename, AudioClip clip)
    {
        if (!filename.ToLower().EndsWith(".wav"))
        {
            filename += ".wav";
        }

        filePath = Path.Combine(Application.persistentDataPath, filename);

        var data = ConvertAudioClipToByteArray(clip);
        if (data == null)
        {
            return false;
        }

        using (var fileStream = CreateEmpty(filePath))
        {
            if (fileStream == null)
            {
                return false;
            }

            fileStream.Write(data, 0, data.Length);
        }

        return true;
    }

    private static byte[] ConvertAudioClipToByteArray(AudioClip clip)
    {
        var samples = new float[clip.samples];
        clip.GetData(samples, 0);

        var byteArray = new byte[samples.Length * 2];
        for (int i = 0; i < samples.Length; i++)
        {
            var sample = (short)(samples[i] * short.MaxValue);
            var bytes = System.BitConverter.GetBytes(sample);
            byteArray[i * 2] = bytes[0];
            byteArray[i * 2 + 1] = bytes[1];
        }

        return byteArray;
    }

    private static FileStream CreateEmpty(string filePath)
    {
        var directoryName = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        var fileStream = File.Create(filePath);
        var header = CreateWaveHeader(44100, 1, 16, fileStream.Length - 8);
        fileStream.Write(header, 0, header.Length);

        return fileStream;
    }

    private static byte[] CreateWaveHeader(int sampleRate, int channels, int bitDepth, long fileSize)
    {
        byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        byte[] chunkSize = System.BitConverter.GetBytes(fileSize - 8);
        byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        byte[] subChunk1Size = System.BitConverter.GetBytes(16);
        short audioFormat = 1; // PCM
        byte[] numChannels = System.BitConverter.GetBytes(channels);
        byte[] sampleRateBytes = System.BitConverter.GetBytes(sampleRate);
        byte[] byteRate = System.BitConverter.GetBytes(sampleRate * channels * bitDepth / 8);
        short blockAlign = (short)(channels * bitDepth / 8);
        byte[] bitsPerSample = System.BitConverter.GetBytes(bitDepth);
        byte[] subChunk2Id = System.Text.Encoding.UTF8.GetBytes("data");
        byte[] subChunk2Size = System.BitConverter.GetBytes(fileSize - 44);

        var header = new byte[44];
        System.Buffer.BlockCopy(riff, 0, header, 0, 4);
        System.Buffer.BlockCopy(chunkSize, 0, header, 4, 4);
        System.Buffer.BlockCopy(wave, 0, header, 8, 4);
        System.Buffer.BlockCopy(fmt, 0, header, 12, 4);
        System.Buffer.BlockCopy(subChunk1Size, 0, header, 16, 4);
        System.Buffer.BlockCopy(System.BitConverter.GetBytes(audioFormat), 0, header, 20, 2);
        System.Buffer.BlockCopy(numChannels, 0, header, 22, 2);
        System.Buffer.BlockCopy(sampleRateBytes, 0, header, 24, 4);
        System.Buffer.BlockCopy(byteRate, 0, header, 28, 4);
        System.Buffer.BlockCopy(System.BitConverter.GetBytes(blockAlign), 0, header, 32, 2);
        System.Buffer.BlockCopy(bitsPerSample, 0, header, 34, 2);
        System.Buffer.BlockCopy(subChunk2Id, 0, header, 36, 4);
        System.Buffer.BlockCopy(subChunk2Size, 0, header, 40, 4);

        return header;
    }
}