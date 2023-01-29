using System.Reflection;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.GameData;

public class SkillInfoProvider : ISkillInfoProvider
{
    private IDictionary<MagicSkillId, SkillInfo> _skills;

    public SkillInfoProvider(string skillsInfoDataFile)
    {
        _skills = new Dictionary<MagicSkillId, SkillInfo>();

        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "L2sniffer.GameData.Data." + skillsInfoDataFile;
        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        using (var reader = new StreamReader(stream))
        {
            var isFirst = true;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (isFirst)
                {
                    isFirst = false;
                    continue;
                }

                var parts = line.Split('\t');
                var skillInfo = new SkillInfo()
                {
                    Id = uint.Parse(parts[0]),
                    Level = ushort.Parse(parts[1]),
                    Name = parts[2][2..^2],
                    Description = parts[3],
                    DescriptionAdd1 = parts[4],
                    DescriptionAdd2 = parts[5]
                };
                _skills[new MagicSkillId() { Id = skillInfo.Id, Level = skillInfo.Level }] = skillInfo;
            }
        }
    }

    public SkillInfo GetSkillInfo(MagicSkillId skill)
    {
        return _skills[skill];
    }

    public bool TryGetSkillInfo(MagicSkillId skill, out SkillInfo skillInfo)
    {
        return _skills.TryGetValue(skill, out skillInfo);
    }
}