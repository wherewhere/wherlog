using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Wherlog.Helpers;

namespace Wherlog.Pages.ToolsPages
{
    public partial class BiliBiliCardPage : IAsyncDisposable
    {
        private const string JAVASCRIPT_FILE = $"./{nameof(Pages)}/{nameof(ToolsPages)}/{nameof(BiliBiliCardPage)}.razor.js";
        private readonly HttpClient client = new();

        private IJSObjectReference _jsModule;

        private Task<string> GetApiAsync() =>
            string.IsNullOrEmpty(id)
                ? Task.FromResult(string.Empty)
                : client.GetStringAsync(GetApi(id, type));

        private async Task<string> CreateCardAsync(string json, string imageProxy, string id, string type, string infoTypes, string theme)
        {
            Dictionary<string, string> message = GetMessage(json, id, type);
            switch (output)
            {
                case "components":
                    return CreateElement(imageProxy, infoTypes, message, theme);
                case "html":
                    _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
                    return await _jsModule.InvokeAsync<string>(
                        "createCard",
                        imageProxy,
                        infoTypes,
                        new
                        {
                            vid = message.TryGetValue("vid", out string vid) ? vid : null,
                            type = message.TryGetValue("type", out string _type) ? _type : null,
                            title = message.TryGetValue("title", out string title) ? title : null,
                            author = message.TryGetValue("author", out string author) ? author : null,
                            cover = message.TryGetValue("cover", out string cover) ? cover : null,
                            duration = message.TryGetValue("duration", out string duration) ? duration : null,
                            views = message.TryGetValue("views", out string views) ? views : null,
                            danmakus = message.TryGetValue("danmakus", out string danmakus) ? danmakus : null,
                            comments = message.TryGetValue("comments", out string comments) ? comments : null,
                            favorites = message.TryGetValue("favorites", out string favorites) ? favorites : null,
                            coins = message.TryGetValue("coins", out string coins) ? coins : null,
                            likes = message.TryGetValue("likes", out string likes) ? likes : null
                        },
                        theme);
                default:
                    return string.Empty;
            }
        }

        private static Dictionary<string, string> GetMessage(string json, string id, string type)
        {
            if (string.IsNullOrEmpty(json)) { return []; }
            JsonElementNode token = JsonDocument.Parse(json).RootElement;
            Dictionary<string, string> message;
            switch (type)
            {
                case "video":
                video:
                    message = GetVideoMessage(id, token);
                    break;
                case "article":
                article:
                    message = GetArticleMessage(id, token);
                    break;
                case "user":
                    message = GetUserMessage(id, token);
                    break;
                case "live":
                    message = GetLiveMessage(id, token);
                    break;
                case "bangumi":
                bangumi:
                    message = GetBangumiMessage(id, token);
                    break;
                case "audio":
                audio:
                    message = GetAudioMessage(id, token);
                    break;
                case "dynamic":
                    message = GetDynamicMessage(id, token);
                    break;
                case "favorite":
                    message = GetFavoriteMessage(id, token);
                    break;
                case "album":
                    message = GetAlbumMessage(id, token);
                    break;
                default:
                    type = id?.Length >= 2 ? id[2..].ToLowerInvariant() : id;
                    switch (type)
                    {
                        case "cv":
                            goto article;
                        case "md":
                            goto bangumi;
                        case "au":
                            goto audio;
                        case "bv" or "av" or _:
                            goto video;
                    }

            }
            return message;
        }

        private static string GetApi(string id, string type)
        {
            if (string.IsNullOrEmpty(id)) { return string.Empty; }
            switch (type)
            {
                case "video":
                video:
                    (string Id, string Type) vid = GetVid(id);
                    return $"https://api.bilibili.com/x/web-interface/view?{vid.Type}={vid.Id}";
                case "article":
                article:
                    string cvid = id.StartsWith("cv", StringComparison.OrdinalIgnoreCase) ? id[2..] : id;
                    return $"https://api.bilibili.com/x/article/viewinfo?id={cvid}";
                case "user":
                    return $"https://api.bilibili.com/x/web-interface/card?mid={id}";
                case "live":
                    return $"https://api.live.bilibili.com/room/v1/Room/get_info?room_id={id}";
                case "bangumi":
                bangumi:
                    string mdid = id.StartsWith("md", StringComparison.OrdinalIgnoreCase) ? id[2..] : id;
                    return $"https://api.bilibili.com/pgc/review/user?media_id={mdid}";
                case "audio":
                audio:
                    string auid = id.StartsWith("au", StringComparison.OrdinalIgnoreCase) ? id[2..] : id;
                    return $"https://api.bilibili.com/audio/music-service-c/web/song/info?sid={auid}";
                case "dynamic":
                    return $"https://api.vc.bilibili.com/dynamic_svr/v1/dynamic_svr/get_dynamic_detail?dynamic_id={id}";
                case "favorite":
                    return $"https://api.bilibili.com/x/v3/fav/folder/info?media_id={id}";
                case "album":
                    return $"https://api.vc.bilibili.com/link_draw/v1/doc/detail?doc_id={id}";
                default:
                    type = id?.Length >= 2 ? id[2..].ToLowerInvariant() : id;
                    switch (type)
                    {
                        case "cv":
                            goto article;
                        case "md":
                            goto bangumi;
                        case "au":
                            goto audio;
                        case "bv" or "av" or _:
                            goto video;
                    }
            }
        }

        private static Dictionary<string, string> GetVideoMessage(string id, JsonElementNode token)
        {
            const string type = "video";
            if (token != null)
            {
                long? code = token["code"]?.GetInt64();
                switch (code)
                {
                    case 0 when token["data"] is JsonElementNode data:
                        Dictionary<string, string> message = [];
                        message["vid"] = data["bvid"]?.GetString();
                        message["type"] = type;
                        message["title"] = data["title"]?.GetString();
                        message["author"] = data["owner"]?["name"]?.GetString();
                        message["cover"] = data["pic"]?.GetString();
                        message["duration"] = FormatSecondsToTime(data["duration"]?.GetInt64());
                        if (data["stat"] is JsonElementNode stat)
                        {
                            message["views"] = FormatLargeNumber(stat["view"]?.GetInt64());
                            message["danmakus"] = FormatLargeNumber(stat["danmaku"]?.GetInt64());
                            message["comments"] = FormatLargeNumber(stat["reply"]?.GetInt64());
                            message["favorites"] = FormatLargeNumber(stat["favorite"]?.GetInt64());
                            message["coins"] = FormatLargeNumber(stat["coin"]?.GetInt64());
                            message["likes"] = FormatLargeNumber(stat["like"]?.GetInt64());
                        }
                        return message;

                    case 0:
                        Console.WriteLine($"Failed to get bilibili {type} {id}");
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", "出错了！" }
                        };

                    case -403:
                        Warn(code, token["message"]?.GetString());
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", $"权限不足！{code}" }
                        };

                    case -404:
                        Warn(code, token["message"]?.GetString());
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", $"视频不存在！{code}" }
                        };

                    case 0xF232:
                        Warn(code, token["message"]?.GetString());
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", $"稿件不可见！{code}" }
                        };

                    case 0xF234:
                        Warn(code, token["message"]?.GetString());
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", $"稿件审核中！{code}" }
                        };

                    case -400:
                    default:
                        Warn(code, token["message"]?.GetString());
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", $"出错了！{code}" }
                        };
                }

                void Warn(long? code, string message)
                {
                    Console.WriteLine($"Failed to get bilibili {type} {id}: {{ code: {code}, message: {message} }}");
                }
            }
            return null;
        }

        private static Dictionary<string, string> GetArticleMessage(string id, JsonElementNode token)
        {
            const string type = "article";
            if (token != null)
            {
                long? code = token["code"]?.GetInt64();
                switch (code)
                {
                    case 0 when token["data"] is JsonElementNode data:
                        Dictionary<string, string> message = [];
                        message["vid"] = id?.StartsWith("cv", StringComparison.OrdinalIgnoreCase) == true ? id[2..] : id;
                        message["type"] = type;
                        message["title"] = data["title"]?.GetString();
                        message["author"] = data["author_name"]?.GetString();
                        message["cover"] = data["banner_url"]?.GetString();
                        if (data["stats"] is JsonElementNode stats)
                        {
                            message["views"] = FormatLargeNumber(stats["view"]?.GetInt64());
                            message["comments"] = FormatLargeNumber(stats["reply"]?.GetInt64());
                            message["favorites"] = FormatLargeNumber(stats["favorite"]?.GetInt64());
                            message["coins"] = FormatLargeNumber(stats["coin"]?.GetInt64());
                            message["likes"] = FormatLargeNumber(stats["like"]?.GetInt64());
                        }
                        return message;

                    case 0:
                        Console.WriteLine($"Failed to get bilibili {type} {id}");
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", "出错了！" }
                        };

                    case -404:
                        Warn(code, token["message"]?.GetString());
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", $"专栏不存在！{code}" }
                        };

                    case -400:
                    default:
                        Warn(code, token["message"]?.GetString());
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", $"出错了！{code}" }
                        };
                }

                void Warn(long? code, string message)
                {
                    Console.WriteLine($"Failed to get bilibili {type} {id}: {{ code: {code}, message: {message} }}");
                }
            }
            return null;
        }

        private static Dictionary<string, string> GetUserMessage(string id, JsonElementNode token)
        {
            const string type = "user";
            if (token != null)
            {
                long? code = token["code"]?.GetInt64();
                switch (code)
                {
                    case 0 when token["data"] is JsonElementNode data
                             && data["card"] is JsonElementNode card:
                        Dictionary<string, string> message = [];
                        message["vid"] = card["mid"]?.GetString();
                        message["type"] = type;
                        message["title"] = $"{card["name"]}\n{card["sign"]}";
                        message["author"] = card["name"]?.GetString();
                        message["cover"] = card["face"]?.GetString();
                        message["views"] = FormatLargeNumber(card["fans"]?.GetInt64());
                        message["likes"] = FormatLargeNumber(data["like_num"]?.GetInt64());
                        return message;

                    case 0:
                        Console.WriteLine($"Failed to get bilibili {type} {id}");
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", "出错了！" }
                        };

                    case -400:
                    default:
                        Warn(code, token["message"]?.GetString());
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", $"出错了！{code}" }
                        };
                }

                void Warn(long? code, string message)
                {
                    Console.WriteLine($"Failed to get bilibili {type} {id}: {{ code: {code}, message: {message} }}");
                }
            }
            return null;
        }

        private static Dictionary<string, string> GetLiveMessage(string id, JsonElementNode token)
        {
            const string type = "live";
            if (token != null)
            {
                long? code = token["code"]?.GetInt64();
                switch (code)
                {
                    case 0 when token["data"] is JsonElementNode data:
                        Dictionary<string, string> message = [];
                        message["vid"] = data["room_id"]?.GetInt64().ToString();
                        message["type"] = type;
                        message["title"] = data["title"]?.GetString();
                        message["cover"] = data["user_cover"]?.GetString();
                        message["views"] = FormatLargeNumber(data["online"]?.GetInt64());
                        return message;

                    case 0:
                        Console.WriteLine($"Failed to get bilibili {type} {id}");
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", "出错了！" }
                        };

                    case 1:
                        Warn(code, token["message"]?.GetString());
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", $"房间不存在！{code}" }
                        };

                    default:
                        Warn(code, token["message"]?.GetString());
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", $"出错了！{code}" }
                        };
                }

                void Warn(long? code, string message)
                {
                    Console.WriteLine($"Failed to get bilibili {type} {id}: {{ code: {code}, message: {message} }}");
                }
            }
            return null;
        }

        private static Dictionary<string, string> GetBangumiMessage(string id, JsonElementNode token)
        {
            const string type = "bangumi";
            if (token != null)
            {
                long? code = token["code"]?.GetInt64();
                switch (code)
                {
                    case 0 when (token["result"]?["media"]) is JsonElementNode media:
                        Dictionary<string, string> message = [];
                        message["vid"] = media["media_id"]?.GetInt64().ToString();
                        message["type"] = type;
                        message["title"] = media["title"]?.GetString();
                        message["author"] = media["type_name"]?.GetString();
                        message["cover"] = media["horizontal_picture"]?.GetString();
                        message["favorites"] = FormatLargeNumber(media["rating"]?["score"]?.GetInt64());
                        return message;

                    case 0:
                        Console.WriteLine($"Failed to get bilibili {type} {id}");
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", "出错了！" }
                        };

                    case -404:
                        Warn(code, token["message"]?.GetString());
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", $"番剧不存在！{code}" }
                        };

                    case -400:
                    default:
                        Warn(code, token["message"]?.GetString());
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", $"出错了！{code}" }
                        };
                }

                void Warn(long? code, string message)
                {
                    Console.WriteLine($"Failed to get bilibili {type} {id}: {{ code: {code}, message: {message} }}");
                }
            }
            return null;
        }

        private static Dictionary<string, string> GetAudioMessage(string id, JsonElementNode token)
        {
            const string type = "audio";
            if (token != null)
            {
                long? code = token["code"]?.GetInt64();
                switch (code)
                {
                    case 0 when token["data"] is JsonElementNode data:
                        Dictionary<string, string> message = [];
                        message["vid"] = data["id"]?.GetInt64().ToString();
                        message["type"] = type;
                        message["title"] = data["title"]?.GetString();
                        message["author"] = data["author"]?.GetString();
                        message["cover"] = data["cover"]?.GetString();
                        message["duration"] = FormatSecondsToTime(data["duration"]?.GetInt64());
                        if (data["statistic"] is JsonElementNode statistic)
                        {
                            message["views"] = FormatLargeNumber(statistic["play"]?.GetInt64());
                            message["comments"] = FormatLargeNumber(statistic["comment"]?.GetInt64());
                            message["favorites"] = FormatLargeNumber(statistic["collect"]?.GetInt64());
                        }
                        message["coins"] = FormatLargeNumber(data["coin_num"]?.GetInt64());
                        return message;

                    case 0:
                        Console.WriteLine($"Failed to get bilibili {type} {id}");
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", "出错了！" }
                        };

                    default:
                        Warn(code, token["message"]?.GetString());
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", $"出错了！{code}" }
                        };
                }

                void Warn(long? code, string message)
                {
                    Console.WriteLine($"Failed to get bilibili {type} {id}: {{ code: {code}, message: {message} }}");
                }
            }
            return null;
        }

        private static Dictionary<string, string> GetDynamicMessage(string id, JsonElementNode token)
        {
            const string type = "dynamic";
            if (token != null)
            {
                long? code = token["code"]?.GetInt64();
                switch (code)
                {
                    case 0 when (token["data"]?["card"]) is JsonElementNode data:
                        Dictionary<string, string> message = [];
                        if (data["desc"] is JsonElementNode desc)
                        {
                            message["vid"] = desc["dynamic_id"]?.GetInt64().ToString();
                            message["author"] = desc["user_profile"]?["info"]?["uname"]?.GetString();
                            message["views"] = FormatLargeNumber(desc["view"]?.GetInt64());
                            message["comments"] = FormatLargeNumber(desc["comment"]?.GetInt64());
                            message["likes"] = FormatLargeNumber(desc["like"]?.GetInt64());
                            long? _type = desc["type"]?.GetInt64();
                            JsonElementNode card =
                                _type is 1 or 4 or 2 or 8 or 64
                                ? JsonDocument.Parse(data["card"]?.GetString()).RootElement
                                : null;
                            switch (_type)
                            {
                                case 1:
                                case 4:
                                    message["title"] = card["item"]?["content"]?.GetString();
                                    message["cover"] = card["user"]?["face"]?.GetString();
                                    break;
                                case 2:
                                    message["title"] = card["item"]?["description"]?.GetString();
                                    message["cover"] = card["item"]?["pictures"]?.FirstOrDefault()?["img_src"]?.GetString();
                                    break;
                                case 8:
                                    message["title"] = card["dynamic"]?.GetString();
                                    message["cover"] = card["pic"]?.GetString();
                                    break;
                                case 64:
                                    message["title"] = card["title"]?.GetString();
                                    message["cover"] = card["image_urls"]?.FirstOrDefault()?.GetString();
                                    break;
                                default:
                                    message["title"] = $"{desc["user_profile"]?["info"]?["uname"]?.GetString()} 的动态";
                                    message["cover"] = desc["user_profile"]?["info"]?["face"]?.GetString();
                                    break;
                            }
                        }
                        return message;

                    case 0:
                        Console.WriteLine($"Failed to get bilibili {type} {id}");
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", "出错了！" }
                        };

                    default:
                        Warn(code, token["message"]?.GetString());
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", $"出错了！{code}" }
                        };
                }

                void Warn(long? code, string message)
                {
                    Console.WriteLine($"Failed to get bilibili {type} {id}: {{ code: {code}, message: {message} }}");
                }
            }
            return null;
        }

        private static Dictionary<string, string> GetFavoriteMessage(string id, JsonElementNode token)
        {
            const string type = "favorite";
            if (token != null)
            {
                long? code = token["code"]?.GetInt64();
                switch (code)
                {
                    case 0 when token["data"] is JsonElementNode data:
                        Dictionary<string, string> message = [];
                        message["vid"] = data["id"]?.GetInt64().ToString();
                        message["type"] = type;
                        message["title"] = data["title"]?.GetString();
                        message["author"] = data["upper"]?["name"]?.GetString();
                        message["cover"] = data["cover"]?.GetString();
                        if (data["cnt_info"] is JsonElementNode cnt_info)
                        {
                            message["views"] = FormatLargeNumber(cnt_info["play"]?.GetInt64());
                            message["favorites"] = FormatLargeNumber(cnt_info["collect"]?.GetInt64());
                        }
                        return message;

                    case 0:
                        Console.WriteLine($"Failed to get bilibili {type} {id}");
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", "出错了！" }
                        };

                    case -403:
                        Warn(code, token["message"]?.GetString());
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", $"权限不足！{code}" }
                        };

                    case -400:
                    default:
                        Warn(code, token["message"]?.GetString());
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", $"出错了！{code}" }
                        };
                }

                void Warn(long? code, string message)
                {
                    Console.WriteLine($"Failed to get bilibili {type} {id}: {{ code: {code}, message: {message} }}");
                }
            }
            return null;
        }

        private static Dictionary<string, string> GetAlbumMessage(string id, JsonElementNode token)
        {
            const string type = "album";
            if (token != null)
            {
                long? code = token["code"]?.GetInt64();
                switch (code)
                {
                    case 0 when token["data"] is JsonElementNode data
                             && data["item"] is JsonElementNode item:
                        Dictionary<string, string> message = [];
                        message["vid"] = item["doc_id"]?.GetInt64().ToString();
                        message["type"] = type;
                        message["title"] = item["title"]?.GetString();
                        message["author"] = data["user"]?["name"]?.GetString();
                        message["cover"] = item["pictures"]?.FirstOrDefault()?["img_src"]?.GetString();
                        message["views"] = FormatLargeNumber(item["view_count"]?.GetInt64());
                        message["comments"] = FormatLargeNumber(item["comment_count"]?.GetInt64());
                        message["favorites"] = FormatLargeNumber(item["collect_count"]?.GetInt64());
                        message["likes"] = FormatLargeNumber(item["like_count"]?.GetInt64());
                        return message;

                    case 0:
                        Console.WriteLine($"Failed to get bilibili {type} {id}");
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", "出错了！" }
                        };

                    case 110001:
                        Warn(code, token["message"]?.GetString());
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", $"相册不存在！{code}" }
                        };

                    default:
                        Warn(code, token["message"]?.GetString());
                        return new Dictionary<string, string> {
                            { "vid", id },
                            { "type", type },
                            { "title", $"出错了！{code}" }
                        };
                }

                void Warn(long? code, string message)
                {
                    Console.WriteLine($"Failed to get bilibili {type} {id}: {{ code: {code}, message: {message} }}");
                }
            }
            return null;
        }

        private static string CreateElement(string imageProxy, string infoTypes, Dictionary<string, string> attributes, string theme)
        {
            StringBuilder builder = new("<bilibili-card");
            if (attributes.TryGetValue("vid", out string vid) && !string.IsNullOrEmpty(vid))
            {
                _ = builder.Append(" vid=\"").Append(vid).Append('"');
            }
            if (attributes.TryGetValue("type", out string type) && !string.IsNullOrEmpty(type))
            {
                _ = builder.Append(" type=\"").Append(type).Append('"');
            }
            if (attributes.TryGetValue("title", out string title) && !string.IsNullOrEmpty(title))
            {
                _ = builder.Append(" title=\"").Append(title).Append('"');
            }
            if (attributes.TryGetValue("author", out string author) && !string.IsNullOrEmpty(author))
            {
                _ = builder.Append(" author=\"").Append(author).Append('"');
            }
            if (attributes.TryGetValue("cover", out string cover) && !string.IsNullOrEmpty(cover))
            {
                _ = builder.Append(" cover=\"").Append(cover).Append('"');
            }
            if (attributes.TryGetValue("duration", out string duration) && !string.IsNullOrEmpty(duration))
            {
                _ = builder.Append(" duration=\"").Append(duration).Append('"');
            }
            if (attributes.TryGetValue("views", out string views) && !string.IsNullOrEmpty(views))
            {
                _ = builder.Append(" views=\"").Append(views).Append('"');
            }
            if (attributes.TryGetValue("danmakus", out string danmakus) && !string.IsNullOrEmpty(danmakus))
            {
                _ = builder.Append(" danmakus=\"").Append(danmakus).Append('"');
            }
            if (attributes.TryGetValue("comments", out string comments) && !string.IsNullOrEmpty(comments))
            {
                _ = builder.Append(" comments=\"").Append(comments).Append('"');
            }
            if (attributes.TryGetValue("favorites", out string favorites) && !string.IsNullOrEmpty(favorites))
            {
                _ = builder.Append(" favorites=\"").Append(favorites).Append('"');
            }
            if (attributes.TryGetValue("coins", out string coins) && !string.IsNullOrEmpty(coins))
            {
                _ = builder.Append(" coins=\"").Append(coins).Append('"');
            }
            if (attributes.TryGetValue("likes", out string likes) && !string.IsNullOrEmpty(likes))
            {
                _ = builder.Append(" likes=\"").Append(likes).Append('"');
            }
            if (!string.IsNullOrEmpty(infoTypes))
            {
                _ = builder.Append(" info-types=\"").Append(infoTypes).Append('"');
            }
            if (!string.IsNullOrEmpty(imageProxy))
            {
                _ = builder.Append(" image-proxy=\"").Append(imageProxy).Append('"');
            }
            if (!string.IsNullOrEmpty(theme))
            {
                _ = builder.Append(" theme=\"").Append(theme).Append('"');
            }
            return builder.Append("></bilibili-card>").ToString();
        }

        private static (string Id, string Type) GetVid(string id) =>
            id.StartsWith("BV", StringComparison.OrdinalIgnoreCase)
                ? ((string Id, string Type))(id, "bvid")
                : id.StartsWith("av", StringComparison.OrdinalIgnoreCase)
                    ? ((string Id, string Type))(id[2..], "aid")
                    : ulong.TryParse(id, out ulong num)
                        ? ((string Id, string Type))(num.ToString(), "aid")
                        : ((string Id, string Type))($"BV{id}", "bvid");

        private static string FormatLargeNumber(long? number) =>
            number is long num
                ? (num >= 100000000)
                    ? $"{num / 100000000d:F1}亿"
                    : (num >= 10000)
                        ? $"{num / 10000d:F1}万"
                        : num.ToString()
                : string.Empty;

        private static string FormatSecondsToTime(long? second)
        {
            if (second is not long sec) { return string.Empty; }
            TimeSpan time = TimeSpan.FromSeconds(sec);
            StringBuilder builder = new();
            ulong hour = (ulong)time.TotalHours;
            if (hour > 0)
            {
                _ = builder.Append(hour.ToString("D2")).Append(':');
            }
            _ = builder.Append(time.Minutes.ToString("D2")).Append(':')
                       .Append(time.Seconds.ToString("D2"));
            return builder.ToString();
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                client.Dispose();
                if (_jsModule != null)
                {
                    await _jsModule.DisposeAsync();
                    _jsModule = null;
                }
                GC.SuppressFinalize(this);
            }
            catch (Exception ex) when (ex is JSDisconnectedException or
                                       OperationCanceledException)
            {
                // The JSRuntime side may routinely be gone already if the reason we're disposing is that
                // the client disconnected. This is not an error.
                Logger.LogWarning(ex, "JSRuntime has already disconnected. {message} (0x{hResult:X})", ex.GetMessage(), ex.HResult);
            }
        }

        private class JsonElementNode(in JsonElement element) : IEnumerable<JsonElementNode>
        {
            private readonly JsonElement element = element;

            public JsonElementNode this[string value] => element.TryGetProperty(value, out JsonElement property) == true ? property : null;

            public string GetString() => element.GetString();

            public long? GetInt64() => element.TryGetInt64(out long value) ? value : null;

            public override string ToString() => element.ToString();

            public IEnumerator<JsonElementNode> GetEnumerator() => element.EnumerateArray().Select<JsonElement, JsonElementNode>(x => x).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public static implicit operator JsonElementNode(in JsonElement element) => new(element);

            public static implicit operator JsonElement(JsonElementNode node) => node.element;
        }
    }
}
