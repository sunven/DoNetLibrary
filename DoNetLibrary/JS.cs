using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using System.Collections;
using System.Data;


namespace Common
{
    public enum DialogOperate
    {
        View = 0,
        Edit = 1,
        Add = 2,
        ApproveView = 3,
        Other = 4
    }
    /// <summary>
    /// JS弹出窗口的各种参数信息
    /// </summary>
    public class DialogInfo
    {
        public DialogInfo()
        {
            ExtendKeyValues = new Hashtable();
        }
        /// <summary>
        /// 页面URL，可传入完整的URL或页面名称，如：
        /// Default.aspx / http://localhost/Default.aspx
        /// 注意：如果使用完整URL，则不能设置RelativeURL = false;
        /// </summary>
        public String URL;
        /// <summary>
        /// 弹出页面处理数据的ID
        /// </summary>
        public String DataID = "0";

        public Int32 dialogWidth = 800;

        public Int32 dialogHeight = 700;
        /// <summary>
        /// 操作参数，默认浏览
        /// </summary>
        public DialogOperate dialogOperate = DialogOperate.View;
        public Boolean scrollbars = false;
        public Boolean status = false;
        public Boolean help = false;
        /// <summary>
        /// 扩展传入值/参数对的HashTable
        /// </summary>
        public Hashtable ExtendKeyValues = null;
        /// <summary>
        /// 缺省为相对路径，打开当前页面相应目录下的弹出页面。
        /// 如果false则，自动生成绝对路径
        /// </summary>
        public Boolean AbsoluteUri = false;
    }
    /// <summary>
    /// 保护构造,封闭类  
    /// </summary>
  static  public class JS
    {
        //protected JS()
        //{ }

        /// <summary>
        /// 把状态转为对应的样式名
        /// </summary>
        /// <param name="FlowState">草稿，已提交，审批中，不批准，批准，已办理，不办理</param>
        /// <returns>CG,YTJ,SPZ,BPZ,PZ,YBL,BBL</returns>
        public static string FitFlowState(string FlowState)
        {
            switch (FlowState)
            {
                case "草稿":
                    return "CG";
                case "已提交":
                    return "YTJ";
                case "不批准":
                    return "BPZ";
                case "批准":
                    return "PZ";
                case "已办理":
                    return "YBL";
                case "不办理":
                    return "BBL";
                default:
                    return "SPZ";
            }
        }

        /// <summary>
        /// 直接关闭窗口（不执行其他客户端代码）
        /// </summary>
        public static void ClosePage()
        {
            HttpContext.Current.Response.Write(@"<script>window.opener=null;window.close();</script>");
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="page">this</param>
        public static void ClosePage(Page page)
        {
            AddScript(page, "window.opener=null;window.close();", "close");
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="page">this</param>
        public static void ClosePage(string msg, Page page)
        {
            AddScript(page, "window.alert('" + msg + "');window.close();", "close");
        }


        /// <summary>
        /// 弹出
        /// </summary>
        /// <param name="page">this </param>
        /// <param name="Msg">提示文本</param>
        public static void Alert(Page page, string Msg)
        {
            AddScript(page, "alert(\"" + Msg.Replace('\'', '‘') + "\");", "alert" + DateTime.Now.Ticks);
        }

        /// <summary>
        /// 在当前 System.Web.HttpContext 请求的对象用Response.Write方法将脚本注册到页面的弹出提示框方法 LIN 2006-03-12
        /// </summary>
        /// <param name="Msg">提示文本</param>
        /// <remarks>Add by LIN IN 2006-03-11 在当前 HTTP 请求 的对象页面弹出提示框</remarks>
        public static void alertResponse(string Msg)
        {
            StringBuilder sb = new StringBuilder("<script language=\"javascript\">");
            sb.Append("alert('" + Msg + "');");
            sb.Append("</script>");
            System.Web.HttpContext.Current.Response.Write(sb.ToString());
        }
        /// <summary>
        /// 添加脚本
        /// </summary>
        /// <param name="page">this</param>
        /// <param name="Scripts">脚本</param>
        /// <param name="Name">脚本名</param>        
        public static void AddScript(Page page, string Scripts, string Name)
        {

            //StringBuilder sb = new StringBuilder("<script language=\"javascript\">");
            //sb.Append(Scripts);
            //sb.Append("</script>");
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), Guid.NewGuid().ToString(), Scripts, true);

            //page.ClientScript.RegisterStartupScript(page.GetType(), Name, sb.ToString());
        }

        public static void AddScript(Page page, string Scripts)
        {
            AddScript(page, Scripts, "");
        }

         

        public static string GetScript(string Scripts)
        {
            StringBuilder sb = new StringBuilder("<script language=\"javascript\">");
            sb.Append(Scripts);
            sb.Append("</script>");
            return sb.ToString();
        }

        /// <summary>
        /// 打开窗口
        /// </summary>
        /// <param name="page">this</param>
        /// <param name="URL">地址</param>
        /// <param name="Target">目标框架</param>
        public static void open(Page page, string URL, string Target)
        {
            AddScript(page, "window.open('" + URL + "','" + Target + "');", "redirect");
        }

        /// <summary>
        /// 在页面顶部添加脚本
        /// </summary>
        /// <param name="page">this</param>
        /// <param name="values">脚本</param>
        public static void AddHead(Page page, string script)
        {
            StringBuilder sb = new StringBuilder("<script language=\"javascript\">");
            sb.Append(script);
            sb.Append("</script>");
            page.Response.Write(sb.ToString());
        }

        /// <summary>
        /// 弹出提示并跳转页面
        /// </summary>
        /// <param name="page">this</param>
        /// <param name="Msg">提示信息</param>
        /// <param name="URL">跳转地址</param>
        /// <param name="Target">目标框架</param>
        public static void AlertAndRedirect(Page page, string Msg, string URL)
        {
            AddScript(page, "alert(\"" + Msg.Replace('\'', '‘') + "\");location.href='" + URL + "';", "alert" + DateTime.Now.Ticks);
        }
        /// <summary>
        /// 弹出提示并返回
        /// </summary>
        /// <param name="page">this</param>
        /// <param name="Msg">提示信息</param>
        public static void AlertAndBack(Page page, string Msg)
        {
            AddScript(page, "alert(\"" + Msg.Replace('\'', '‘') + "\");history.back();", "alert" + DateTime.Now.Ticks);
        }
        /// <summary>
        /// 生成唯一号
        /// </summary>
        /// <param name="type">前缀</param>
        /// <returns>号码</returns>
        public static string GetNumber(string type)
        {
            int intNumber;
            unchecked
            {
                System.Random rand = new Random(Math.Abs((int)DateTime.Now.Ticks));
                intNumber = rand.Next(1, 9999);
            }
            if (type.Length < 2)
            {
                type = "YJ";
            }

            string strType = type.Substring(0, 2).ToUpper();
            string strDate = DateTime.Now.ToString("yyMMddHHmmssfff");
            string strRand = intNumber.ToString("0000");

            return strType + strDate + strRand;    //+ strUser
        }

        /// <summary>
        /// 弹出新增窗口
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="dialogWidth"></param>
        /// <param name="dialogHeight"></param>
        /// <returns></returns>
        public static string ShowModalDialog(string URL, string dialogWidth, string dialogHeight)
        {
            return ShowModalDialog(URL, DialogOperate.Add, "0", dialogWidth, dialogHeight, false, false, false, null);
        }

        /// <summary>
        /// 弹出修改页面，使用默认大小
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="dialogOperate"></param>
        /// <param name="dataID"></param>
        /// <returns></returns>
        public static string ShowModalDialog(string URL, DialogOperate dialogOperate, String dataID)
        {
            return ShowModalDialog(URL, dialogOperate, dataID, "800", "800", false, false, false, null);
        }

        /// <summary>
        /// 使用哈希表存放用户扩展参数，生成request字符串
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="dialogOperate"></param>
        /// <param name="dataID"></param>
        /// <param name="dialogWidth"></param>
        /// <param name="dialogHeight"></param>
        /// <param name="Args">HashTable扩展值参</param>
        /// <returns>eg.?Operation=2&Key=Value&Key=Vaue</returns>
        public static string ShowModalDialog(string URL, DialogOperate dialogOperate, String dataID, string dialogWidth, string dialogHeight, Hashtable Args)
        {
            return ShowModalDialog(URL, dialogOperate, dataID, dialogWidth, dialogHeight, false, false, false, Args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="dialogOperate"></param>
        /// <param name="dataID"></param>
        /// <param name="dialogWidth"></param>
        /// <param name="dialogHeight"></param>
        /// <returns></returns>
        public static string ShowModalDialog(string URL, DialogOperate dialogOperate, string dataID, string dialogWidth, string dialogHeight)
        {
            return ShowModalDialog(URL, dialogOperate, dataID, dialogWidth, dialogHeight, false, false, false, null);
        }

        public static string ShowModalDialog(string URL, DialogOperate dialogOperate, string dataID,
                                                                string dialogWidth, string dialogHeight,
                                                                bool scrollbars, bool status, bool help, Hashtable Args)
        {

            string retVal = "";
            string strReqArgs = "?Operation=" + Convert.ToInt32(dialogOperate).ToString() + "&ID=" + dataID;
            if (Args != null)
            {
                foreach (DictionaryEntry de in Args)
                {
                    strReqArgs += "&" + de.Key.ToString() + "=" + de.Value.ToString();
                }
            }

            string sOptions = "";

            // 滚动条
            if (scrollbars)
                sOptions += "scrollbars:yes;";
            else
                sOptions += "scrollbars:no;";

            // 帮助
            if (help)
                sOptions += "help:yes;";
            else
                sOptions += "help:no;";

            // 状态栏
            if (status)
                sOptions += "status:yes;";
            else
                sOptions += "status:no;";

            sOptions += "dialogWidth:" + dialogWidth + "px;";
            sOptions += "dialogHeight:" + dialogHeight + "px;";


            if (URL != "")
            {
                retVal = " return window.showModalDialog('" + URL + strReqArgs + "','__NEW__','" + sOptions + "')";
                return retVal;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 原来的方式调用起来不够方便，改变生成方式重新定义
        /// 陈叔雄 2006-03-02
        /// </summary>
        /// <param name="dialogInfo"></param>
        /// <returns></returns>
        public static String ShowModalDialog(DialogInfo dialogInfo)
        {
            String retVal = "";
            String strKeyValues = "?Operation=" + Convert.ToInt32(dialogInfo.dialogOperate).ToString() +
                                "&ID=" + dialogInfo.DataID;
            if (dialogInfo.ExtendKeyValues != null)
            {
                foreach (DictionaryEntry deKeyValue in dialogInfo.ExtendKeyValues)
                {
                    strKeyValues += "&" + deKeyValue.Key.ToString() + "=" + deKeyValue.Value.ToString();
                }
            }

            String sOptions = "";

            // 滚动条
            if (dialogInfo.scrollbars)
                sOptions += "scrollbars:yes;";
            else
                sOptions += "scrollbars:no;";

            // 帮助
            if (dialogInfo.help)
                sOptions += "help:yes;";
            else
                sOptions += "help:no;";

            // 状态栏
            if (dialogInfo.status)
                sOptions += "status:yes;";
            else
                sOptions += "status:no;";

            sOptions += "dialogWidth:" + dialogInfo.dialogWidth.ToString() + "px;";
            sOptions += "dialogHeight:" + dialogInfo.dialogHeight.ToString() + "px;";

            // 
            if (!String.IsNullOrEmpty(dialogInfo.URL))
            {
                // 使用绝对路径
                String path = "";
                if (dialogInfo.AbsoluteUri)
                {
                    Uri Url = HttpContext.Current.Request.Url;
                    path = Url.Scheme + "://" + Url.Authority + "/" + Url.Segments[1];
                    path += dialogInfo.URL;
                }
                else
                {
                    path = dialogInfo.URL;
                }
                retVal = "   window.showModalDialog('" + path + strKeyValues + "','__NEW__','" + sOptions + "')";
                return retVal;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 注册全选/取消全选脚本
        /// </summary>
        public static void RegisterCheckScript(Page p)
        {
            p.ClientScript.RegisterStartupScript(typeof(Page), "jsCheck", @"<script>function checkFormAll(chk){form = document.getElementById(""myForm"");for(var i=0;i<form.elements.length;i++)
{if(form.elements[i].type == ""checkbox""){form.elements[i].checked = chk;}}}</script>");
        }

        public static void RetainScrollPosition(Page myPage, string strBodyName)
        {
            RegisterSetBodyID(myPage, strBodyName);

            StringBuilder saveScrollPosition = new StringBuilder("<script language='javascript'>function saveScrollPosition() {if (document.body.id != 'strBodyName' || document.body.id == null) {return;}document.forms[0].__SCROLLPOS.value = strBodyName.scrollTop;} strBodyName.onscroll=saveScrollPosition;</script>");
            StringBuilder setScrollPosition = new StringBuilder("<script language='javascript'>function setScrollPosition() {if (document.body.id !='strBodyName'|| document.body.id == null) {return;} strBodyName.scrollTop = [@__SCROLLPOS];}strBodyName.onload=setScrollPosition;</script>");

            myPage.ClientScript.RegisterHiddenField("__SCROLLPOS", "0");
            saveScrollPosition.Replace("strBodyName", strBodyName);
            myPage.ClientScript.RegisterStartupScript(typeof(Page), "saveScroll", saveScrollPosition.ToString());

            if (myPage.IsPostBack)
            {
                //TODO:这里造成页面上出现莫名其妙的0或者其它数字，要修改
                setScrollPosition.Replace("strBodyName", strBodyName);
                setScrollPosition.Replace("[@__SCROLLPOS]", myPage.Request["__SCROLLPOS"]);
                myPage.ClientScript.RegisterStartupScript(typeof(Page), "setScroll", setScrollPosition.ToString());
            }
        }


        public static string Escape(string str)
        {
            if (str == null)
                return String.Empty;

            StringBuilder sb = new StringBuilder();
            int len = str.Length;

            for (int i = 0; i < len; i++)
            {
                char c = str[i];
                string cesc = "";

                try { cesc = Uri.HexEscape(c); }
                catch { sb.Append(c); }
                finally { sb.Append(cesc); }
            }

            return sb.ToString();
        }

        public static string UnEscape(string str)
        {
            if (str == null)
                return String.Empty;

            StringBuilder sb = new StringBuilder();
            int len = str.Length;
            int i = 0;
            while (i != len)
            {
                if (Uri.IsHexEncoding(str, i))
                    sb.Append(Uri.HexUnescape(str, ref i));
                else
                    sb.Append(str[i++]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 注册页面的默认Body ID名称(防脚本错误)
        /// </summary>
        /// <param name="myPage"></param>
        /// <param name="strBodyName"></param>
        /// <remarks></remarks>
        public static void RegisterSetBodyID(System.Web.UI.Page myPage, string strBodyName)
        {
            myPage.ClientScript.RegisterStartupScript(typeof(Page), "SetBodyID", "<script language='javascript'>document.body.id = '" + strBodyName + "'</script>");
        }


        /// <summary>
        /// 获取多选的 CheckBoxList 或 ListBox 的选择值字符串
        /// </summary>
        /// <param name="control">控件ID</param>
        /// <returns>        '选项1','选项2','选项3'        </returns>
        public static String GetListControlCollection(ListControl control)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ListItem var in control.Items)
            {
                if (var.Selected)
                {
                    sb.Append('\'');
                    sb.Append(var.Value);
                    sb.Append("',");
                }
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 把字符串转换成HTML编码的字符
        /// </summary>
        /// <returns></returns>
        public static string Encode(string id)
        {
            return System.Web.HttpUtility.UrlEncode(id);
        }

        public static string Encode(string id, Encoding e)
        {
            return System.Web.HttpUtility.UrlEncode(id, e);
        }

        /// <summary>
        /// 把HTML编码的字符解码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string Decode(string id)
        {
            return System.Web.HttpUtility.UrlDecode(id);
        }

        /// <summary>
        /// 使控件得到焦点
        /// </summary>
        /// <param name="key">脚本名</param>
        /// <param name="id">控件名称</param>
        public static void SetFocus(Page page, string key, string id)
        {
            //page.RegisterStartupScript(key, "<script>function setFoces(){var o = document.getElementById('" + id + "');o.focus();}; setFoces();</script>");
        }

        /// <summary>
        /// 检查字符串是否可以转换为数字类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckHasInt(string str)
        {
            decimal iResult = 0;
            return decimal.TryParse(str, out iResult);
        }

        #region 客户端脚本的函数
        /// <summary>
        /// 格式化客户端的输出.替换掉单引号和双引号,并把回车换行符颠倒是替换成为客户端的JavaScript回车换行符.		
        /// </summary>
        /// <param name="oldValue"></param>
        public static string ReplaceQuotationMark(string oldValue)
        {
            return oldValue.Replace("\'", "”").Replace("\"", "”").Replace("\r", "\\r").Replace("\n", "\\n");
        }

        /// <summary>
        /// 判断送入的字符是否数值型(含小数)
        /// </summary>
        /// <param name="sNumStr"></param>
        /// <returns></returns>
        public static bool IsNumnic(string sNumStr)
        {
            try
            {
                decimal num = Convert.ToDecimal(sNumStr);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 是否是号码(如  0012300310056)   ZYL
        /// </summary>
        /// <param name="sNumStr"></param>
        /// <returns></returns>
        public static bool IsNumber(string sNumStr)
        {
            Char[] charArray = sNumStr.ToCharArray();
            foreach (Char c in charArray)
            {
                if (!Char.IsNumber(c))
                {
                    return false;
                }
            }
            return true;
        }



        /// <summary>
        /// 将LinkButton、Button注册Onclick事件，并通过返回提示的操作进行下一步的执行
        /// 编辑人：Fisher 
        /// 日期：2005-06-07
        /// </summary>
        /// <param name="btnLinkButton"></param>
        /// <param name="sFunctionName"></param>
        /// <param name="sTipInfor"></param>		
        public static void ConfigEventScripRegister(LinkButton btnLinkButton, string sFunctionName)
        {
            btnLinkButton.Attributes.Add("onClick", "return " + sFunctionName + "();");
        }

        public static void ConfigEventScripRegister(Button btnButton, string sFunctionName)
        {
            btnButton.Attributes.Add("onClick", "return " + sFunctionName + "();");
        }


        /// <summary>
        /// ASP.NET的加密方式
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string PswdFormat(string str, string format = "MD5")
        {
            string returnstr = str;
            if (format == "SHA1")
            {
                returnstr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1");
            }

            if (format == "MD5")
            {
                //暂时还是不要加密好了  －－BY MYJ
                //returnstr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
                returnstr = str;
            }
            return returnstr;
        }

        /// <summary>
        /// 检查是否存在中文
        /// </summary>
        /// <param name="str">需要验证的字符串</param>
        /// <returns>true:包含中文 false:不包含中文</returns>
        public static bool ChineseStr(string str)
        {
            int intText;
            char[] chrNo = str.ToCharArray();
            for (int i = 0; i < chrNo.Length; i++)
            {
                intText = Convert.ToInt32(chrNo[i]);
                if (!(intText >= Convert.ToInt32('A') && intText <= Convert.ToInt32('Z') || intText >= Convert.ToInt32('a') && intText <= Convert.ToInt32('z') || intText >= Convert.ToInt32('0') && intText <= Convert.ToInt32('9')))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 处理页面输出，防止出现非法字符 ' 号引起的数据库操作错误。
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string InputText(string inputString, int maxLength)
        {
            StringBuilder retVal = new StringBuilder();

            // check incoming parameters for null or blank string
            if ((inputString != null) && (inputString != String.Empty))
            {
                inputString = inputString.Trim();

                //chop the string incase the client-side max length
                //fields are bypassed to prevent buffer over-runs
                if (inputString.Length > maxLength)
                    inputString = inputString.Substring(0, maxLength);

                //convert some harmful symbols incase the regular
                //expression validators are changed
                for (int i = 0; i < inputString.Length; i++)
                {
                    switch (inputString[i])
                    {
                        case '"':
                            retVal.Append("&quot;");
                            break;
                        case '<':
                            retVal.Append("&lt;");
                            break;
                        case '>':
                            retVal.Append("&gt;");
                            break;
                        default:
                            retVal.Append(inputString[i]);
                            break;
                    }
                }
                // Replace single quotes with white space
                retVal = retVal.Replace("'", "’");
            }

            return retVal.ToString();

        }
        public static string InputText(string inputString, int maxLength, bool bReplaceSqlCharToNOChar)
        {
            string sInputText = InputText(inputString, maxLength);

            if (bReplaceSqlCharToNOChar == true)
            {
                sInputText = sInputText.Replace("[", "[[]");
                sInputText = sInputText.Replace("%", "[%]");
                sInputText = sInputText.Replace("_", "[_]");
                sInputText = sInputText.Replace("'", "''");
                return sInputText;
            }
            else
            {
                sInputText = sInputText.Replace("[", "");
                sInputText = sInputText.Replace("%", "");
                sInputText = sInputText.Replace("_", "");
                sInputText = sInputText.Replace("'", "");
                sInputText = sInputText.Replace("=", "");
                return sInputText;
            }
        }


        /// <summary>
        /// 隐藏多余的字符
        /// </summary>
        /// <param name="strSource">原始字符串</param>
        /// <param name="len">显示的最大长度</param>
        /// <returns></returns>
        public static string HideLongString(string strSource, int len)
        {
            if (strSource == null) return "";
            if (strSource.Length > len)
            {
                return strSource.Substring(0, len - 3) + "...";
            }
            else
            {
                return strSource;
            }
        }

        #endregion

        /// <summary>
        /// 获取多选的 CheckBoxList 或 ListBox 的选择值字符串
        /// </summary>
        /// <param name="control">控件ID</param>
        /// <param name="bString">是否字符串</param>
        /// <returns>        '选项1','选项2','选项3'        </returns>
        public static String GetListControlCollection(ListControl control, bool bString)
        {
            if (bString)
            {//字符串格式
                return GetListControlCollection(control);
            }

            StringBuilder sb = new StringBuilder();
            foreach (ListItem var in control.Items)
            {
                if (var.Selected)
                {
                    sb.Append(var.Value);
                    sb.Append(",");
                }
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 给控件增加属性
        /// </summary>
        /// <param name="c">控件</param>
        /// <param name="name_">属性名</param>
        /// <param name="value_">属性值</param>
        /// <example>
        /// JS.AddAttributesToControl(TextBox1,"oncopy","return false;"); --生成的客户端html中增加 oncopy="return false;"
        /// </example>
        public static void AddAttributesToControl(IAttributeAccessor c, string name_, string value_)
        {

            c.SetAttribute(name_, value_);

        }
        /// <summary>
        /// 向一个控件添加一个确认对话框.(如果取消则不提交服务器端)
        /// </summary>
        /// <param name="c">需要添加确认的控件</param>
        /// <param name="msg">对话框中显示的文本</param>
        public static void AddConfirmToControl(IAttributeAccessor c, string msg)
        {
            if (c is HtmlControl)
            {
                ((HtmlControl)c).Attributes.Add("onclick", "return confirm('" + msg + "');");
            }
            else if (c is WebControl)
            {
                ((WebControl)c).Attributes.Add("onclick", "return confirm('" + msg + "');");
            }
        }



        /// <summary>
        /// 绑定双击
        /// </summary>
        /// <param name="DBClickControl">双击的控件ID</param>
        /// <param name="gotoUrl">跳转URL</param>
        /// <param name="target">目标窗体</param>
        public static void BindDblClick(WebControl DBClickControl, string gotoUrl, string target = "_self")
        {
            BindDblClickScript(DBClickControl, "window.open('" + gotoUrl + "','" + target + "');");
        }

        /// <summary>
        /// 绑定双击
        /// </summary>
        /// <param name="DBClickControl">双击的控件ID</param>
        /// <param name="script">自定义脚本</param>
        private static void BindDblClickScript(WebControl DBClickControl, string script)
        {
            DBClickControl.Attributes["ondblclick"] = "" + script + "";
        }


        public static void BindHook(Page page, KeyCode code, string strStript)
        {
            AddScript(page, "DocumentHook.AddHook(" + (int)code + ",\"" + strStript + "\");\r\n");
        }


        public static void RemoveHook(Page page, KeyCode keyCode)
        {
            AddScript(page, "DocumentHook.RemoveHook(" + (int)keyCode + ");\r\n");
        }


        public enum KeyCode
        {
            /// <summary>
            /// ESC键  
            /// </summary>
            VK_ESCAPE = 27,

            /// <summary>
            /// 回车键  
            /// </summary>
            VK_RETURN = 13,
            /// <summary>
            /// TAB键 
            /// </summary>
            VK_TAB = 9,
            /// <summary>
            /// Caps Lock键
            /// </summary>
            VK_CAPITAL = 20,
            /// <summary>
            /// Shift键
            /// </summary>
            VK_SHIFT = 10,
            /// <summary>
            /// Ctrl键
            /// </summary>
            VK_CONTROL = 17,
            /// <summary>
            /// Alt键
            /// </summary>
            VK_MENU = 18,
            /// <summary>
            /// 空格键
            /// </summary>
            VK_SPACE = 32,
            /// <summary>
            /// 退格键
            /// </summary>
            VK_BACK = 8,
            /// <summary>
            /// 左徽标键
            /// </summary>
            VK_LWIN = 91,
            /// <summary>
            /// F2键
            /// </summary>
            VK_F2 = 113,
            /// <summary>
            ///  F3键
            /// </summary>
            VK_F3 = 114,
            /// <summary>
            ///  F4键
            /// </summary>
            VK_F4 = 115,
            /// <summary>
            ///  F8键
            /// </summary>
            VK_F8 = 119,
            VK_F9 = 120,
            /*
             * 下面有需要,自己改
右徽标键：      VK_LWIN    (92)
鼠标右键快捷键：VK_APPS    (93)

Insert键： VK_INSERT  (45)
Home键：   VK_HOME    (36)
Page Up：  VK_PRIOR   (33)
PageDown： VK_NEXT    (34)
End键：    VK_END     (35)
Delete键： VK_DELETE  (46)

方向键(←)： VK_LEFT  (37)
方向键(↑)： VK_UP    (38)
方向键(→)： VK_RIGHT (39)
方向键(↓)： VK_DOWN  (40)

F1键：  VK_F1  (112)



F5键：  VK_F5  (116)
F6键：  VK_F6  (117)
F7键：  VK_F7  (118)

F9键：  VK_F9  (120)
F10键： VK_F10 (121)
F11键： VK_F11 (122)
F12键： VK_F12 (123)

Num Lock键： VK_NUMLOCK  (144)
小键盘0：    VK_NUMPAD0  (96)
小键盘1：    VK_NUMPAD0  (97)
小键盘2：    VK_NUMPAD0  (98)
小键盘3：    VK_NUMPAD0  (99)
小键盘4：    VK_NUMPAD0  (100)
小键盘5：    VK_NUMPAD0  (101)
小键盘6：    VK_NUMPAD0  (102)
小键盘7：    VK_NUMPAD0  (103)
小键盘8：    VK_NUMPAD0  (104)
小键盘9：    VK_NUMPAD0  (105)
小键盘.：    VK_DECIMAL  (110)
小键盘*：    VK_MULTIPLY (106)
小键盘+：    VK_MULTIPLY (107)
小键盘-：    VK_SUBTRACT (109)
小键盘/：    VK_DIVIDE   (111)

Pause Break键： VK_PAUSE  (19)
Scroll Lock键： VK_SCROLL (145)
             */
        }

    }
}
