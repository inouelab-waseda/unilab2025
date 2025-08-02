using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace unilab2025
{
    public partial class MiniGame_Nono_Stage : Form
    {
        #region メンバ変数
        public int StageId { get; set; }    // ステージレベル
        private int[,] solutionData;        // 正答
        private int[,] playerData;          // プレイヤーの解答
        private int cellSize;               // マス目サイズ
        #endregion

        public MiniGame_Nono_Stage()
        {
            InitializeComponent();
        }
    }
}
