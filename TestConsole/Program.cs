using AddIns;

List<string>? myLines = await CustomFileInfo.GetFilesLinesAsync(
    "",
    "abra",
    @"TestTextFiles\EmptyFile.txt",
    @"TestTextFiles\OneSpaceFile.txt",
    @"TestTextFiles\SomeLines1.txt",
    @"TestTextFiles\SomeLines2.txt"
) as List<string>;


foreach (var item in myLines!)
{
    Console.WriteLine(item);
}
int i = 0;
