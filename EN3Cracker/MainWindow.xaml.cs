using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EN3Cracker
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public string EnPath { get; set; }

        public MainWindow()
        {
            CancellationTokenSource findTaskCts = new CancellationTokenSource();
            CancellationToken ftct = findTaskCts.Token;

            InitializeComponent();

            this.Loaded += async (object sender, RoutedEventArgs e) =>
            {
                startpatch.IsEnabled = false;
                startbtntext.Text = "正在寻找程序目录...";

                await Task.Run(() =>
                {
                    string softName = @"EasiNote\Main\EasiNote.exe";
                    bool complete = false;
                    bool canceled = false;
                    try
                    {
                        // 访问 32 位注册表
                        using (RegistryKey lmRegKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                        using (RegistryKey uninstallKey = lmRegKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"))
                        {
                            if (uninstallKey != null)
                            {
                                foreach (var subKeyName in uninstallKey.GetSubKeyNames())
                                {
                                    if (findTaskCts.IsCancellationRequested)
                                    {
                                        canceled = true;
                                        return;
                                    }

                                    using (RegistryKey childKey = uninstallKey.OpenSubKey(subKeyName))
                                    {
                                        if (childKey != null)
                                        {
                                            foreach (var valueName in childKey.GetValueNames())
                                            {
                                                var exePath = Convert.ToString(childKey.GetValue(valueName));
                                                if (!string.IsNullOrEmpty(exePath) && exePath.Contains(softName))
                                                {
                                                    this.Dispatcher.Invoke(() =>
                                                    {
                                                        EnPath = Path.GetDirectoryName(exePath);
                                                    });
                                                    complete = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (complete) break;
                                }
                            }
                        }
                    }
                    catch
                    {
                        Finish(2);
                        return;
                    }

                    if (complete || canceled)
                    {
                        Finish(0);
                    }
                    else
                    {
                        Finish(1);
                    }

                    void Finish(int finishCode)
                    {
                        // 0: 成功, 1: 未找到, 2: 错误
                        this.Dispatcher.Invoke(() =>
                        {
                            switch (finishCode)
                            {
                                case 0:
                                    enpathbox.Text = EnPath;
                                    startbtntext.Text = "激活";
                                    break;
                                case 1:
                                    startbtntext.Text = "未自动找到希沃白板程序目录";
                                    break;
                                case 2:
                                    startbtntext.Text = "寻找程序目录时发生错误";
                                    break;
                            }
                            startpatch.IsEnabled = true;
                        });
                    }
                }, ftct);
            };

            this.enpathbox.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                findTaskCts.Cancel();
            };
        }

        private void startpatch_Click(object sender, RoutedEventArgs e)
        {
            string currentPath = enpathbox.Text;
            if (string.IsNullOrEmpty(currentPath) || !File.Exists(Path.Combine(currentPath, "EasiNote.exe")))
            {
                if (MessageBox.Show("该希沃白板路径似乎不正确，是否继续？", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Crack();
                }
            }
            else
            {
                Crack();
            }

            void Crack()
            {
                string targetDirectory = enpathbox.Text;
                string fileName = "Cvte.Platform.Basic.dll";
                string destPath = Path.Combine(targetDirectory, fileName);

                try
                {
                    string sourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Crackfiles", fileName);
                    if (!File.Exists(sourcePath))
                    {
                        sourcePath = Path.Combine(".", "Crackfiles", fileName);
                    }

                    if (!File.Exists(sourcePath))
                    {
                        throw new FileNotFoundException("找不到补丁源文件", sourcePath);
                    }

                    // 确保目标文件夹存在 
                    if (!Directory.Exists(targetDirectory))
                    {
                        Directory.CreateDirectory(targetDirectory);
                    }

                    // 执行拷贝 
                    File.Copy(sourcePath, destPath, true);

                    startbtntext.Text = "激活成功, 即将自动退出";
                    Task.Run(() =>
                    {
                        Thread.Sleep(1000);
                        Environment.Exit(0);
                    });
                }
                catch (FileNotFoundException ex)
                {
                    MessageBox.Show($"找不到补丁文件 ({fileName})，请确保程序包完整。\n错误: {ex.Message}", "激活失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"文件正在使用中，请关闭相关程序后再试。\n原因: {ex.Message}", "激活失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("没有权限写入该目录，请尝试以管理员身份运行此程序。", "激活失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"发生未知错误: {ex.Message}", "激活失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void browse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "选择希沃白板安装目录"
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                enpathbox.Text = dialog.FileName;
            }
        }
    }
}
