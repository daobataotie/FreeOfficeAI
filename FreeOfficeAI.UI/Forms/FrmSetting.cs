using FreeOfficeAI.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeOfficeAI.UI.Forms
{
    public partial class FrmSetting : Form
    {
        public FrmSetting()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            InitControls();
        }

        private void InitControls()
        {
            var config = ConfigSetting.GetSetting();

            cobModelName.DropDownStyle = ComboBoxStyle.DropDownList;
            cobModelName.Items.Add("deepseek-r1:14b");
            cobModelName.Items.Add("codellama:34b");
            cobModelName.Items.Add("qwen2.5-coder:32b");

            cobFrame.DropDownStyle = ComboBoxStyle.DropDownList;
            cobFrame.DataSource = Enum.GetNames(typeof(LLMFrame));
            cobFrame.SelectedIndexChanged += CobFrame_SelectedIndexChanged;

            if (config.IsLocal)
                rdbLocal.Checked = true;
            else
                rdbApiKey.Checked = true;

            txtAddress.Text = config.ApiUrl;
            cobFrame.SelectedIndex = cobFrame.Items.IndexOf(config.Frame.ToString());
            cobModelName.SelectedIndex = cobModelName.Items.IndexOf(config.ModelName);
        }

        private void CobFrame_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cobFrame.SelectedItem?.ToString() == LLMFrame.Ollama.ToString())
            {
                //读取模型列表
                //if (!string.IsNullOrWhiteSpace(txtAddress.Text))
                //{
                //    var names = OllamaApi.GetModelList(txtAddress.Text);
                //}
            }
        }

        private void btnSaveModel_Click(object sender, EventArgs e)
        {
            if (!ValidateControls())
                return;

            var config = new ConfigSetting();
            config.IsLocal = rdbLocal.Checked;
            config.ApiUrl = txtAddress.Text;
            config.Frame = (LLMFrame)Enum.Parse(typeof(LLMFrame), cobFrame.Text);
            config.ModelName = cobModelName.Text;

            if (ConfigSetting.SaveSetting(config, out string errorMsg))
                MessageBox.Show("保存成功！");
            else
                MessageBox.Show(errorMsg, "保存成功！");
        }

        private void btnConnTestLocal_Click(object sender, EventArgs e)
        {
            if (!ValidateControls())
                return;

            try
            {
                //以同步方式阻塞UI，防止点击测试后，关闭设置页面
                if (OllamaApi.TestConnection(txtAddress.Text, cobModelName.Text).Result)
                {
                    MessageBox.Show("连接成功！");
                }
                else
                {
                    MessageBox.Show("连接失败！");
                }
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions[0] is TaskCanceledException tce && tce.CancellationToken.IsCancellationRequested)
                    MessageBox.Show("请求超时！", "连接失败");
                else
                    MessageBox.Show(ex.InnerExceptions[0].Message, "连接失败");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "连接失败");
            }
        }


        public bool ValidateControls()
        {
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("请输入地址！");
                return false;
            }

            if (cobFrame.SelectedIndex == -1)
            {
                MessageBox.Show("请选择框架！");
                return false;
            }

            if (cobModelName.SelectedIndex == -1)
            {
                MessageBox.Show("请选择模型名！");
                return false;
            }

            return true;
        }
    }
}
