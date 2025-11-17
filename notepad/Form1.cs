using System;
using System.IO;
using System.Windows.Forms;

namespace notepad
{
    public partial class Form1 : Form
    {
        private string? _currentFilePath;

        public Form1()
        {
            InitializeComponent();
            UpdateTitle();
            UpdateStatus();
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ConfirmDiscardChanges())
            {
                return;
            }

            editor.Clear();
            _currentFilePath = null;
            editor.Modified = false;
            UpdateTitle();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ConfirmDiscardChanges())
            {
                return;
            }

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    editor.Text = File.ReadAllText(openFileDialog.FileName);
                    _currentFilePath = openFileDialog.FileName;
                    editor.Modified = false;
                    UpdateTitle();
                    UpdateStatus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"Dosya açılamadı.\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(forceDialog: true);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editor.CanUndo)
            {
                editor.Undo();
            }
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editor.CanRedo)
            {
                editor.Redo();
            }
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.Cut();
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.Copy();
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.Paste();
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.SelectedText = string.Empty;
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.SelectAll();
        }

        private void WordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.WordWrap = wordWrapToolStripMenuItem.Checked;
            if (editor.WordWrap)
            {
                editor.ScrollBars = RichTextBoxScrollBars.Vertical;
            }
            else
            {
                editor.ScrollBars = RichTextBoxScrollBars.Both;
            }
        }

        private void FontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog.Font = editor.Font;
            if (fontDialog.ShowDialog(this) == DialogResult.OK)
            {
                editor.Font = fontDialog.Font;
            }
        }

        private void TextColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog.Color = editor.ForeColor;
            if (colorDialog.ShowDialog(this) == DialogResult.OK)
            {
                editor.ForeColor = colorDialog.Color;
            }
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void Editor_TextChanged(object sender, EventArgs e)
        {
            UpdateTitle();
            UpdateStatus();
        }

        private void Editor_SelectionChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ConfirmDiscardChanges())
            {
                e.Cancel = true;
            }
        }

        private bool SaveFile(bool forceDialog = false)
        {
            string? targetPath = _currentFilePath;

            if (forceDialog || string.IsNullOrWhiteSpace(targetPath))
            {
                if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
                {
                    return false;
                }

                targetPath = saveFileDialog.FileName;
            }

            try
            {
                File.WriteAllText(targetPath, editor.Text);
                _currentFilePath = targetPath;
                editor.Modified = false;
                UpdateTitle();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Dosya kaydedilemedi.\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool ConfirmDiscardChanges()
        {
            if (!editor.Modified)
            {
                return true;
            }

            var result = MessageBox.Show(this, "Değişiklikleri kaydetmek istiyor musunuz?", "Notepad",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            return result switch
            {
                DialogResult.Yes => SaveFile(),
                DialogResult.No => true,
                _ => false
            };
        }

        private void UpdateTitle()
        {
            var fileName = string.IsNullOrWhiteSpace(_currentFilePath)
                ? "Adsız"
                : Path.GetFileName(_currentFilePath);

            var suffix = editor.Modified ? "*" : string.Empty;
            Text = $"{fileName}{suffix} - Notepad";
        }

        private void UpdateStatus()
        {
            if (!statusStrip.Visible)
            {
                return;
            }

            int selectionIndex = editor.SelectionStart;
            int line = editor.GetLineFromCharIndex(selectionIndex);
            int column = selectionIndex - editor.GetFirstCharIndexOfCurrentLine();
            statusLabel.Text = $"Satır {line + 1}, Sütun {column + 1} | Karakter {editor.TextLength}";
        }
    }
}
