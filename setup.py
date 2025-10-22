import os
import zipfile
import subprocess
import shutil
from concurrent.futures import ThreadPoolExecutor, as_completed

FFMPEG_PATH = "ffmpeg.exe"
MAX_THREADS = min(8, os.cpu_count() or 4)

def check_for_soniccd_files():
    target_file = "0341-SonicCD-1.0.0.0.xap"
    target_folder = "Assets"

    current_dir = os.getcwd()
    file_path = os.path.join(current_dir, target_file)
    folder_path = os.path.join(current_dir, target_folder)

    file_exists = os.path.isfile(file_path)
    folder_exists = os.path.isdir(folder_path)

    print(f"Looking in: {current_dir}\n")

    if file_exists:
        print(f"Found file: {target_file}")
    else:
        print(f"Missing file: {target_file}")

    if folder_exists:
        print(f"Found folder: {target_folder}")
    else:
        print(f"Missing folder: {target_folder}")

    return file_exists, folder_exists, file_path

def extract_xap(xap_path, output_folder):
    print(f"\nExtracting '{xap_path}' to '{output_folder}'...")
    with zipfile.ZipFile(xap_path, 'r') as zip_ref:
        zip_ref.extractall(output_folder)
    print("Extraction complete.")

def convert_wma_to_mp3(wma_file):
    mp3_file = os.path.splitext(wma_file)[0] + ".mp3"
    subprocess.run([FFMPEG_PATH, "-y", "-i", wma_file, mp3_file],
                   stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL, check=True)
    os.remove(wma_file)

def clean_and_convert_music(music_folder):
    if not os.path.exists(music_folder):
        return

    for root, _, files in os.walk(music_folder):
        for f in files:
            if f.lower().endswith(".xnb"):
                os.remove(os.path.join(root, f))

    wma_files = [os.path.join(root, f)
                 for root, _, files in os.walk(music_folder)
                 for f in files if f.lower().endswith(".wma")]

    if wma_files:
        print("\nConverting Music files...")
        with ThreadPoolExecutor(max_workers=MAX_THREADS) as executor:
            futures = [executor.submit(convert_wma_to_mp3, wma) for wma in wma_files]
            for future in as_completed(futures):
                future.result()
        print("Music conversion complete.")

def copy_to_streaming_assets(source_folder, assets_folder="Assets"):
    streaming_assets_folder = os.path.join(assets_folder, "StreamingAssets")
    os.makedirs(streaming_assets_folder, exist_ok=True)
    print(f"\nCopying files to '{streaming_assets_folder}'...")

    for root, dirs, files in os.walk(source_folder):
        rel_path = os.path.relpath(root, source_folder)
        dest_dir = os.path.join(streaming_assets_folder, rel_path)
        os.makedirs(dest_dir, exist_ok=True)
        for file in files:
            src_file = os.path.join(root, file)
            dst_file = os.path.join(dest_dir, file)
            shutil.copy2(src_file, dst_file)

    print("Copy complete.")

def main():
    file_exists, folder_exists, file_path = check_for_soniccd_files()

    if file_exists:
        extract_folder = "Extracted_XAP"
        os.makedirs(extract_folder, exist_ok=True)
        extract_xap(file_path, extract_folder)

        content_folder = os.path.join(extract_folder, "Content")
        music_folder = os.path.join(content_folder, "Music")
        clean_and_convert_music(music_folder)

        copy_to_streaming_assets(content_folder)

        print("\nRSDKv3Unity is now setup!")
    else:
        print("\nNo XAP file to extract.")

if __name__ == "__main__":
    main()
