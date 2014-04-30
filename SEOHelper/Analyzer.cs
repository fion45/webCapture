using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SEOHelper
{
    public class Analyzer
    {
        public static string NODERGXSTR = "<(\\w+)[^<]+<?/\\1?>";
        public static string NOTHAVEENDTAG = "<(area)?(base)?(basefont)?(br)?(hr)?(col)?(img)?(input)?(link)?(meta)?(isindex)?(param)?\\s[^>]+/?>";
        public static string STRSTARTTAG = "^(\\\")?'?\"?";
        public static string STRENDTAG = "(\\\")?'?\"?$";
        public static string FORWARDTAG = "^../";
        public static string HEADWARDTAG = "^~/";
        public static string COMPLETEHEADTAG = "^(http).+";
        public static string COMENDTAG = "(?<=.com).*";
        public static string NEXTLEVELTAG = "/.+$";
        public static string JAVATAG = "^javascript:";
        public static string ROOTTAG = "^https*://+(([0-9a-zA-Z])+\\.)+\\w+";

        public static string GetTagRegexStr(string tagName)
        {
            string result = string.Format("<{0}[^>]*>[^<>]*</{0}>", tagName);
            return result;
        }

        public static string GetPropertyRegexStr(string proName)
        {
            string result = string.Format("\\s*\"?'?{0}\"?'?\\s*=\\s*([^\"']+?\\s)?(\".+?((\\\")?.?)??\")?('.+?((\\')?.?)??')?", proName);
            return result;
        }

        public static List<Match> GetSpecifyString(string content, string regex)
        {
            List<Match> result = new List<Match>();
            Regex tmpRgx = new Regex(regex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            MatchCollection tmpMC = tmpRgx.Matches(content);
            foreach (Match match in tmpMC)
            {
                result.Add(match);
            }
            return result;
        }

        public static void GetSpecifyString(string content, ref Dictionary<string, List<Match>> regexDic)
        {
            foreach (KeyValuePair<string, List<Match>> pair in regexDic)
            {
                Regex tmpRgx = new Regex(pair.Key, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                MatchCollection tmpMC = tmpRgx.Matches(content);
                foreach (Match match in tmpMC)
                {
                    pair.Value.Add(match);
                }
            }
        }

        public static List<string> GetHtmlNode(string content,string tagName,List<string> replaceStrArr = null)
        {
            if(replaceStrArr == null)
            {
                replaceStrArr = new List<string>();
            }
            //竟然有<img xzzz>这些没有结束符的标签，郁闷
            //解决办法，先去除这些
            int index = replaceStrArr.Count;
            Regex notRgx = new Regex(NOTHAVEENDTAG, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            MatchCollection tmpMC = notRgx.Matches(content);
            int tmpDC = 0;
            foreach (Match match in tmpMC)
            {
                replaceStrArr.Add(match.Value);
                string tmpRStr = string.Format("={0}=", index);
                content = content.Substring(0, match.Index - tmpDC) + tmpRStr + content.Substring(match.Index - tmpDC + match.Length);
                tmpDC += match.Length - tmpRStr.Length;
                ++index;
            }

            Regex getRgx = new Regex("<(" + tagName + ")[^<]+<?/\\1?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            List<string> result = new List<string>();
            Match tmpMatch = getRgx.Match(content);
            while (tmpMatch.Success)
            {
                string tmpStr = tmpMatch.Value;
                tmpStr = ReplaceNumTag(tmpStr, replaceStrArr);
                result.Add(tmpStr);
                tmpMatch = tmpMatch.NextMatch();
            }
            Regex nodeRgx = new Regex(NODERGXSTR, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            tmpMC = nodeRgx.Matches(content);
            tmpDC = 0;
            foreach (Match match in tmpMC)
            {
                replaceStrArr.Add(match.Value);
                string tmpRStr = string.Format("={0}=", index);
                content = content.Substring(0, match.Index - tmpDC) + tmpRStr + content.Substring(match.Index - tmpDC + match.Length);
                tmpDC += match.Length - tmpRStr.Length;
                ++index;
            }
            if (nodeRgx.IsMatch(content))
            {
                List<string> tmpList = GetHtmlNode(content, tagName, replaceStrArr);
                result.AddRange(tmpList);
            }
            return result;
        }

        private static string ReplaceNumTag(string content,List<string>replaceStrArr)
        {
            Regex replaceRgx = new Regex("=(\\d+)=");
            string result = content;
            if (replaceRgx.IsMatch(content))
            {
                MatchCollection tmpMC1 = replaceRgx.Matches(result);
                int tmpAC = 0;
                foreach (Match match in tmpMC1)
                {
                    int tmpIndex = int.Parse(match.Groups[1].Value);
                    result = result.Substring(0, match.Index + tmpAC) + replaceStrArr[tmpIndex] + result.Substring(match.Index + tmpAC + match.Length);
                    tmpAC += replaceStrArr[tmpIndex].Length - match.Length;
                }
                result = ReplaceNumTag(result, replaceStrArr);
            }
            return result;
        }

        public static bool FillUrlString(ref string urlStr,string parUrlStr)
        {
            //if (urlStr.IndexOf("key=critters") > -1)
            //    Console.WriteLine("IN");
            Regex javaTag = new Regex(JAVATAG,RegexOptions.IgnoreCase);
            if (urlStr[0] == '#' || urlStr == "/" || javaTag.IsMatch(urlStr))
                return false;
            Regex headwardTag = new Regex(HEADWARDTAG, RegexOptions.IgnoreCase);
            Regex forwardTag = new Regex(FORWARDTAG, RegexOptions.IgnoreCase);
            Regex completeHeadTag = new Regex(COMPLETEHEADTAG,RegexOptions.IgnoreCase);
            Regex comEndTag = new Regex(COMENDTAG, RegexOptions.IgnoreCase);
            Regex nextLevelTag = new Regex(NEXTLEVELTAG, RegexOptions.IgnoreCase);
            Regex rootTag = new Regex(ROOTTAG,RegexOptions.IgnoreCase);
            if (!completeHeadTag.IsMatch(urlStr) && urlStr[0] != '/' && !headwardTag.IsMatch(urlStr) && !forwardTag.IsMatch(urlStr))
            {
                urlStr = "http://" + urlStr;
            }
            else
            {
                if (urlStr[0] == '/')
                {
                    //全局相对路径
                    Match rMatch = rootTag.Match(parUrlStr);
                    urlStr = rMatch.Value + urlStr;
                }
                else if(urlStr[0] == '?')
                {
                    urlStr = parUrlStr.TrimEnd(new char[] { '/' }) + urlStr;
                }
                else
                {
                    if (completeHeadTag.IsMatch(urlStr))
                        return true;
                    string tmpStr = headwardTag.Match(urlStr).Value;
                    Match tmpMatch = comEndTag.Match(parUrlStr);
                    if (!string.IsNullOrEmpty(tmpStr))
                    {
                        urlStr = parUrlStr.Substring(0, tmpMatch.Index) + urlStr.Substring(1);
                    }
                    else
                    {
                        tmpMatch = forwardTag.Match(urlStr);
                        int index = 0;
                        while (!string.IsNullOrEmpty(tmpMatch.Value))
                        {
                            urlStr = urlStr.Substring(tmpMatch.Value.Length);
                            tmpMatch = forwardTag.Match(urlStr);
                            ++index;
                        }
                        for (int i = 0; i < index; i++)
                        {
                            if (!nextLevelTag.IsMatch(parUrlStr))
                                return false;
                            parUrlStr = nextLevelTag.Replace(parUrlStr, "");
                        }
                        urlStr = parUrlStr.TrimEnd(new char[] { '/' }) + "/" + urlStr.TrimStart(new char[] { '/' });
                    }
                }
            }
            return true;
        }
    }
}
