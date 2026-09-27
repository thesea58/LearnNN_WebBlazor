"""
Script chuyển đổi dữ liệu từ vựng từ file JSON sang CSV chuẩn template import của LearnNN WebBlazor.

Input:  toeic_600_essential_words_en_vi.json
Output: toeic_600_words_import.csv (chuẩn định dạng template_importData.csv)
"""

import csv
import json
import os
import sys
from pathlib import Path

# Đảm bảo in tiếng Việt không bị lỗi charmap trên Windows console
if sys.platform.startswith("win"):
    if hasattr(sys.stdout, "reconfigure"):
        sys.stdout.reconfigure(encoding="utf-8")
    if hasattr(sys.stderr, "reconfigure"):
        sys.stderr.reconfigure(encoding="utf-8")


# Bảng ánh xạ từ loại (Part of Speech) từ viết tắt sang tên đầy đủ
POS_MAPPING = {
    "n": "noun",
    "v": "verb",
    "adj": "adjective",
    "adv": "adverb",
    "phr": "phrase",
    "prep": "preposition",
    "n/v": "noun/verb",
    "v/n": "verb/noun",
    "adj/phr": "adjective/phrase",
    "adv/phr": "adverb/phrase",
    "phr/n": "phrase/noun",
    "phr/v": "phrase/verb",
    "v/adj": "verb/adjective",
}


def map_part_of_speech(pos_raw: str) -> str:
    """Chuẩn hóa từ loại sang định dạng phù hợp với hệ thống."""
    if not pos_raw:
        return ""
    pos_clean = pos_raw.strip().lower()
    return POS_MAPPING.get(pos_clean, pos_clean)


def convert_json_to_csv(
    input_json_path: str,
    output_csv_path: str,
    default_mastered: bool = False
) -> None:
    """
    Đọc dữ liệu từ file JSON và xuất ra file CSV theo chuẩn template của LearnNN WebBlazor.

    Các cột tương ứng trong template_importData.csv:
    - TopicName: Lấy từ 'category' trong JSON
    - Term: Lấy từ 'english' trong JSON
    - Meaning: Lấy từ 'vietnamese' trong JSON
    - Pronunciation: Lấy từ 'english_pronunciation_ipa' trong JSON
    - PartOfSpeech: Lấy từ 'part_of_speech' (được chuẩn hóa qua POS_MAPPING)
    - Example: Lấy từ 'example_sentences.english' trong JSON
    - ExampleTranslation: Lấy từ 'example_sentences.vietnamese' trong JSON
    - IsMastered: Mặc định là 'false'
    """
    input_file = Path(input_json_path)
    output_file = Path(output_csv_path)

    if not input_file.exists():
        raise FileNotFoundError(f"Không tìm thấy file JSON nguồn: {input_file}")

    print(f"Đang đọc file JSON: {input_file.resolve()}...")
    with open(input_file, mode="r", encoding="utf-8") as f:
        data = json.load(f)

    vocabularies = data.get("vocabularies", [])
    if not vocabularies:
        print("Cảnh báo: Không tìm thấy danh sách 'vocabularies' trong file JSON.")
        return

    csv_headers = [
        "TopicName",
        "Term",
        "Meaning",
        "Pronunciation",
        "PartOfSpeech",
        "Example",
        "ExampleTranslation",
        "IsMastered"
    ]

    records = []
    skipped_count = 0
    topic_set = set()

    for idx, item in enumerate(vocabularies, start=1):
        term = (item.get("english") or "").strip()
        meaning = (item.get("vietnamese") or "").strip()
        topic_name = (item.get("category") or "").strip()

        # Bỏ qua nếu thiếu các trường bắt buộc
        if not term or not meaning or not topic_name:
            skipped_count += 1
            print(f"Dòng {idx} bị bỏ qua do thiếu dữ liệu bắt buộc (Term/Meaning/TopicName).")
            continue

        pronunciation = (item.get("english_pronunciation_ipa") or "").strip()
        pos_raw = (item.get("part_of_speech") or "").strip()
        part_of_speech = map_part_of_speech(pos_raw)

        # Lấy câu ví dụ từ object example_sentences
        example_sentences = item.get("example_sentences") or {}
        example = ""
        example_translation = ""

        if isinstance(example_sentences, dict):
            example = (example_sentences.get("english") or "").strip()
            example_translation = (example_sentences.get("vietnamese") or "").strip()

        topic_set.add(topic_name)

        record = {
            "TopicName": topic_name,
            "Term": term,
            "Meaning": meaning,
            "Pronunciation": pronunciation,
            "PartOfSpeech": part_of_speech,
            "Example": example,
            "ExampleTranslation": example_translation,
            "IsMastered": "true" if default_mastered else "false"
        }
        records.append(record)

    # Đảm bảo thư mục đầu ra tồn tại
    output_file.parent.mkdir(parents=True, exist_ok=True)

    # Ghi file CSV với utf-8-sig (BOM) để Excel và C# StreamReader hiển thị đúng tiếng Việt
    print(f"Đang ghi file CSV: {output_file.resolve()}...")
    with open(output_file, mode="w", encoding="utf-8-sig", newline="") as f:
        writer = csv.DictWriter(
            f,
            fieldnames=csv_headers,
            quoting=csv.QUOTE_MINIMAL
        )
        writer.writeheader()
        writer.writerows(records)

    print("\n" + "=" * 50)
    print(" KẾT QUẢ CHUYỂN ĐỔI:")
    print(f"- Tổng số từ đọc được:    {len(vocabularies)}")
    print(f"- Số từ chuyển đổi xong: {len(records)}")
    print(f"- Số dòng bị bỏ qua:     {skipped_count}")
    print(f"- Số lượng chủ đề:       {len(topic_set)}")
    print(f"- File CSV kết quả:      {output_file.name}")
    print("=" * 50)


if __name__ == "__main__":
    current_dir = Path(__file__).resolve().parent

    # Đường dẫn mặc định tương đối với thư mục chứa script
    default_input = current_dir / "toeic_600_essential_words_en_vi.json"
    default_output = current_dir / "toeic_600_words_import.csv"

    input_path = sys.argv[1] if len(sys.argv) > 1 else str(default_input)
    output_path = sys.argv[2] if len(sys.argv) > 2 else str(default_output)

    try:
        convert_json_to_csv(input_path, output_path)
    except Exception as e:
        print(f"Đã xảy ra lỗi: {e}", file=sys.stderr)
        sys.exit(1)
