using System.Reflection;
using ImgToPdfConverter.Utils;

namespace ImgToPdfConverter;

public class Entrypoint
{
    private readonly string _inputPath;
    private readonly string _outputPath;
    private readonly string _generationId;

    public Entrypoint()
    {
        string exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
        string projectRoot = Directory.GetParent(exePath)!.Parent!.Parent!.FullName;

        _inputPath = Path.Combine(projectRoot, "files", "input");
        _outputPath = Path.Combine(projectRoot, "files", "output");

        _generationId = StringUtils.GenerateRandomString(4);
    }

    public void Run()
    {
        FileUtils.EnsureDirectoriesExist([_inputPath, _outputPath]);

        string[] directories = Directory.GetDirectories(_inputPath);

        foreach (string dir in directories)
        {
            string[] files = Directory.GetFiles(dir);

            Array.Sort(files);

            string baseDocName = Path.GetFileName(dir);

            /*  Создать учебный план */
            var upOpts = new ConvertOptions
            {
                OutputPath = Path.Combine(_outputPath, $"{baseDocName}-up__{_generationId}.pdf"),
                InputPagesPathes = files
            };

            PdfConverter.ConvertRotate(upOpts);

            /*  Создать учебный график */
            var ugOpts = new ConvertOptions
            {
                OutputPath = Path.Combine(_outputPath, $"{baseDocName}-ug__{_generationId}.pdf"),
                InputPagesPathes = [files[0]]
            };

            PdfConverter.ConvertRotate(ugOpts);
        }
    }
}
