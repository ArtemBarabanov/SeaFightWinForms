namespace SeaFightWinForms
{
    partial class GameForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            startButton = new PictureBox();
            lazyPctr = new PictureBox();
            closePctr = new PictureBox();
            minimizePctr = new PictureBox();
            WavesTimer = new System.Windows.Forms.Timer(components);
            PlayerLettersBox = new PictureBox();
            OpponentLettersBox = new PictureBox();
            OpponentNumbersBox = new PictureBox();
            PlayerNumbersBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)startButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lazyPctr).BeginInit();
            ((System.ComponentModel.ISupportInitialize)closePctr).BeginInit();
            ((System.ComponentModel.ISupportInitialize)minimizePctr).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PlayerLettersBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)OpponentLettersBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)OpponentNumbersBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PlayerNumbersBox).BeginInit();
            SuspendLayout();
            // 
            // startButton
            // 
            startButton.BackColor = Color.Transparent;
            startButton.BackgroundImage = Properties.Resources.Fight;
            startButton.Location = new Point(565, 210);
            startButton.Name = "startButton";
            startButton.Size = new Size(100, 100);
            startButton.TabIndex = 2;
            startButton.TabStop = false;
            startButton.Visible = false;
            startButton.Click += StartButton_Click;
            startButton.MouseEnter += StartButton_MouseEnter;
            startButton.MouseLeave += StartButton_MouseLeave;
            // 
            // lazyPctr
            // 
            lazyPctr.BackColor = Color.Transparent;
            lazyPctr.Image = Properties.Resources.Lazy;
            lazyPctr.Location = new Point(565, 358);
            lazyPctr.Name = "lazyPctr";
            lazyPctr.Size = new Size(100, 80);
            lazyPctr.TabIndex = 5;
            lazyPctr.TabStop = false;
            lazyPctr.Click += LazyPctr_Click;
            lazyPctr.MouseEnter += LazyPctr_MouseEnter;
            lazyPctr.MouseLeave += LazyPctr_MouseLeave;
            // 
            // closePctr
            // 
            closePctr.BackColor = Color.Transparent;
            closePctr.BackgroundImage = Properties.Resources.Close;
            closePctr.Location = new Point(1138, 2);
            closePctr.Name = "closePctr";
            closePctr.Size = new Size(40, 40);
            closePctr.TabIndex = 9;
            closePctr.TabStop = false;
            closePctr.Click += ClosePctr_Click;
            closePctr.MouseEnter += ClosePctr_MouseEnter;
            closePctr.MouseLeave += ClosePctr_MouseLeave;
            // 
            // minimizePctr
            // 
            minimizePctr.BackColor = Color.Transparent;
            minimizePctr.BackgroundImage = Properties.Resources.Minimize;
            minimizePctr.Location = new Point(1079, 2);
            minimizePctr.Name = "minimizePctr";
            minimizePctr.Size = new Size(40, 40);
            minimizePctr.TabIndex = 10;
            minimizePctr.TabStop = false;
            minimizePctr.Click += MinimizePctr_Click;
            minimizePctr.MouseEnter += MinimizePctr_MouseEnter;
            minimizePctr.MouseLeave += MinimizePctr_MouseLeave;
            // 
            // WavesTimer
            // 
            WavesTimer.Interval = 200;
            WavesTimer.Tick += Waves_Tick;
            // 
            // PlayerLettersBox
            // 
            PlayerLettersBox.BackColor = Color.Transparent;
            PlayerLettersBox.BackgroundImageLayout = ImageLayout.Stretch;
            PlayerLettersBox.Image = Properties.Resources.Letters;
            PlayerLettersBox.Location = new Point(80, 40);
            PlayerLettersBox.Name = "PlayerLettersBox";
            PlayerLettersBox.Size = new Size(400, 40);
            PlayerLettersBox.TabIndex = 11;
            PlayerLettersBox.TabStop = false;
            // 
            // OpponentLettersBox
            // 
            OpponentLettersBox.BackColor = Color.Transparent;
            OpponentLettersBox.Image = Properties.Resources.Letters;
            OpponentLettersBox.Location = new Point(760, 40);
            OpponentLettersBox.Margin = new Padding(4, 3, 4, 3);
            OpponentLettersBox.Name = "OpponentLettersBox";
            OpponentLettersBox.Size = new Size(400, 40);
            OpponentLettersBox.TabIndex = 12;
            OpponentLettersBox.TabStop = false;
            // 
            // OpponentNumbersBox
            // 
            OpponentNumbersBox.BackColor = Color.Transparent;
            OpponentNumbersBox.BackgroundImage = Properties.Resources.Numbers;
            OpponentNumbersBox.Location = new Point(720, 80);
            OpponentNumbersBox.Margin = new Padding(4, 3, 4, 3);
            OpponentNumbersBox.Name = "OpponentNumbersBox";
            OpponentNumbersBox.Size = new Size(40, 400);
            OpponentNumbersBox.TabIndex = 13;
            OpponentNumbersBox.TabStop = false;
            // 
            // PlayerNumbersBox
            // 
            PlayerNumbersBox.BackColor = Color.Transparent;
            PlayerNumbersBox.BackgroundImage = Properties.Resources.Numbers;
            PlayerNumbersBox.Location = new Point(40, 80);
            PlayerNumbersBox.Name = "PlayerNumbersBox";
            PlayerNumbersBox.Size = new Size(40, 400);
            PlayerNumbersBox.TabIndex = 14;
            PlayerNumbersBox.TabStop = false;
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(1200, 760);
            Controls.Add(PlayerNumbersBox);
            Controls.Add(OpponentNumbersBox);
            Controls.Add(OpponentLettersBox);
            Controls.Add(PlayerLettersBox);
            Controls.Add(minimizePctr);
            Controls.Add(closePctr);
            Controls.Add(lazyPctr);
            Controls.Add(startButton);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
            Name = "GameForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += GameForm_Load;
            ((System.ComponentModel.ISupportInitialize)startButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)lazyPctr).EndInit();
            ((System.ComponentModel.ISupportInitialize)closePctr).EndInit();
            ((System.ComponentModel.ISupportInitialize)minimizePctr).EndInit();
            ((System.ComponentModel.ISupportInitialize)PlayerLettersBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)OpponentLettersBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)OpponentNumbersBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)PlayerNumbersBox).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.PictureBox startButton;
        private System.Windows.Forms.PictureBox lazyPctr;
        private System.Windows.Forms.PictureBox closePctr;
        private System.Windows.Forms.PictureBox minimizePctr;
        private System.Windows.Forms.Timer WavesTimer;
        private System.Windows.Forms.PictureBox PlayerLettersBox;
        private System.Windows.Forms.PictureBox OpponentLettersBox;
        private System.Windows.Forms.PictureBox OpponentNumbersBox;
        private System.Windows.Forms.PictureBox PlayerNumbersBox;
    }
}

