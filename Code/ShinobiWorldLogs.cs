using UnityEngine;

namespace ShinobiBox
{
    internal static class ShinobiWorldLogs
    {
        public static void AddWorldLog(string logId, string localeId, string iconPath, Actor actor, string group = "magic")
        {
            if (actor == null) return;

            WorldLogLibrary lib = AssetManager.world_log_library;
            if (lib.get(logId) == null)
            {
                WorldLogAsset asset = new WorldLogAsset
                {
                    id = logId,
                    locale_id = localeId,
                    group = group,
                    path_icon = iconPath,
                    color = Toolbox.color_log_warning,
                    text_replacer = (WorldLogMessage msg, ref string text) =>
                    {
                        text = text.Replace("$name$", msg.getSpecial(1));
                    }
                };
                lib.add(asset);
            }

            var log = new WorldLogMessage(lib.get(logId), actor.getName());
            log.unit = actor;
            log.location = actor.current_tile?.posV3 ?? Vector3.zero;
            log.add();
        }
    }
}
