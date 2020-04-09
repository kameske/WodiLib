using System;
using System.Windows.Forms;
using WodiLib.Common;
using WodiLib.Event;

namespace WodiLibSample.Component
{
    public partial class CommonEventSettingView : UserControl
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 表示コモンイベント
        /// </summary>
        public CommonEvent CommonEvent { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public CommonEventSettingView()
        {
            InitializeComponent();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 表示するコモンイベントをセットする。
        /// </summary>
        /// <param name="commonEvent">[NotNull] コモンイベント</param>
        public void SetCommonEvent(CommonEvent commonEvent)
        {
            if (commonEvent == null) throw new ArgumentNullException(nameof(commonEvent));

            CommonEvent = commonEvent;

            UpdateCommonEventNameText(commonEvent.Name);
            UpdateMemoText(commonEvent.Memo);
            UpdateNumberArg1NameText(commonEvent.NumberArgDescList[0].ArgName);
            UpdateNumberArg2NameText(commonEvent.NumberArgDescList[1].ArgName);
            UpdateNumberArg3NameText(commonEvent.NumberArgDescList[2].ArgName);
            UpdateNumberArg4NameText(commonEvent.NumberArgDescList[3].ArgName);
            UpdateStringArg1NameText(commonEvent.StringArgDescList[0].ArgName);
            UpdateStringArg2NameText(commonEvent.StringArgDescList[1].ArgName);
            UpdateStringArg3NameText(commonEvent.StringArgDescList[2].ArgName);
            UpdateStringArg4NameText(commonEvent.StringArgDescList[3].ArgName);
            UpdateBootConditionText(GetBootTypeText(commonEvent.BootCondition.CommonEventBootType));
            UpdateLeftSideText(commonEvent.BootCondition.LeftSide.ToString());
            UpdateRightSideText(commonEvent.BootCondition.RightSide.ToString());
            UpdateBootOperationText(GetOperationText(commonEvent.BootCondition.Operation));
            UpdateReturnVarValueText(commonEvent.IsReturnValue
                ? $"セルフ変数{commonEvent.ReturnVariableIndex.ToString()}"
                : "戻り値なし");
            UpdateReturnVarNameText(commonEvent.IsReturnValue
                ? commonEvent.ReturnValueDescription.ToString()
                : string.Empty);
        }

        /// <summary>
        /// 表示をクリアする。
        /// </summary>
        public void ClearCommonEvent()
        {
            UpdateCommonEventNameText(string.Empty);
            UpdateMemoText(string.Empty);
            UpdateNumberArg1NameText(string.Empty);
            UpdateNumberArg2NameText(string.Empty);
            UpdateNumberArg3NameText(string.Empty);
            UpdateNumberArg4NameText(string.Empty);
            UpdateStringArg1NameText(string.Empty);
            UpdateStringArg2NameText(string.Empty);
            UpdateStringArg3NameText(string.Empty);
            UpdateStringArg4NameText(string.Empty);
            UpdateBootConditionText(string.Empty);
            UpdateLeftSideText(string.Empty);
            UpdateRightSideText(string.Empty);
            UpdateBootOperationText(string.Empty);
            UpdateReturnVarValueText(string.Empty);
            UpdateReturnVarNameText(string.Empty);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private string GetBootTypeText(CommonEventBootType type)
        {
            if (type == CommonEventBootType.Auto) return "自動実行";
            if (type == CommonEventBootType.OnlyCall) return "呼び出しのみ";
            if (type == CommonEventBootType.Parallel) return "並列実行";
            if (type == CommonEventBootType.ParallelAlways) return "並列実行（常時）";

            throw new ArgumentException();
        }

        private string GetOperationText(CriteriaOperator operation)
        {
            if (operation == CriteriaOperator.Equal) return "と同じ";
            if (operation == CriteriaOperator.Above) return "より大きい";
            if (operation == CriteriaOperator.Greater) return "以上";
            if (operation == CriteriaOperator.Less) return "以下";
            if (operation == CriteriaOperator.Below) return "未満";
            if (operation == CriteriaOperator.Not) return "以外";
            if (operation == CriteriaOperator.BitAnd) return "ビット積を満たす";

            throw new ArgumentException();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     UI
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private void UpdateCommonEventNameText(string commonEventName)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) (() => UpdateCommonEventNameText(commonEventName)));
                return;
            }

            txtCommonEventName.Text = commonEventName;
        }

        private void UpdateMemoText(string description)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) (() => UpdateMemoText(description)));
                return;
            }

            txtMemo.Text = description;
        }

        private void UpdateNumberArg1NameText(string argName)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) (() => UpdateNumberArg1NameText(argName)));
                return;
            }

            txtNumberArgName1.Text = argName;
        }

        private void UpdateNumberArg2NameText(string argName)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) (() => UpdateNumberArg2NameText(argName)));
                return;
            }

            txtNumberArgName2.Text = argName;
        }

        private void UpdateNumberArg3NameText(string argName)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) (() => UpdateNumberArg3NameText(argName)));
                return;
            }

            txtNumberArgName3.Text = argName;
        }

        private void UpdateNumberArg4NameText(string argName)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) (() => UpdateNumberArg4NameText(argName)));
                return;
            }

            txtNumberArgName4.Text = argName;
        }

        private void UpdateStringArg1NameText(string argName)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) (() => UpdateStringArg1NameText(argName)));
                return;
            }

            txtStringArgName1.Text = argName;
        }

        private void UpdateStringArg2NameText(string argName)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) (() => UpdateStringArg2NameText(argName)));
                return;
            }

            txtStringArgName2.Text = argName;
        }

        private void UpdateStringArg3NameText(string argName)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) (() => UpdateStringArg3NameText(argName)));
                return;
            }

            txtStringArgName3.Text = argName;
        }

        private void UpdateStringArg4NameText(string argName)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) (() => UpdateStringArg4NameText(argName)));
                return;
            }

            txtStringArgName4.Text = argName;
        }

        private void UpdateBootConditionText(string bootText)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) (() => UpdateBootConditionText(bootText)));
                return;
            }

            txtBootCondition.Text = bootText;
        }

        private void UpdateLeftSideText(string leftSideText)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) (() => UpdateLeftSideText(leftSideText)));
                return;
            }

            txtBootLeftSide.Text = leftSideText;
        }

        private void UpdateRightSideText(string rightSideText)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) (() => UpdateRightSideText(rightSideText)));
                return;
            }

            txtBootRightSide.Text = rightSideText;
        }

        private void UpdateBootOperationText(string operationText)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) (() => UpdateBootOperationText(operationText)));
                return;
            }

            txtBootOperation.Text = operationText;
        }

        private void UpdateReturnVarValueText(string valueText)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) (() => UpdateReturnVarValueText(valueText)));
                return;
            }

            txtReturnVarValue.Text = valueText;
        }

        private void UpdateReturnVarNameText(string nameText)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) (() => UpdateReturnVarNameText(nameText)));
                return;
            }

            txtReturnVarName.Text = nameText;
        }
    }
}