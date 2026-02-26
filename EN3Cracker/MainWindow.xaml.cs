using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EN3Cracker
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	/// 
	public partial class MainWindow : Window
	{
		public string enpath { get; set; }
		public MainWindow()
		{
			CancellationTokenSource findtaskcts = new CancellationTokenSource();
			CancellationToken ftct = findtaskcts.Token;

			InitializeComponent();

			this.Loaded += async (object sender, RoutedEventArgs e) =>
			{
				startbtntext.Text = "正在寻找程序目录...";

				await Task.Run(() =>
				{
					//C:\Program Files (x86)\Seewo\EasiNote\Main\EasiNote.exe
					string softname = "EasiNote\\Main\\EasiNote.exe";
					bool complete = false;
					bool canceled = false;
					try
					{
						RegistryKey lmregkey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);//访问32位注册表
						RegistryKey uninstallkey = lmregkey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");//从卸载中寻找安装路径
						foreach (var subkey in uninstallkey.GetSubKeyNames())//遍历子键
						{
							RegistryKey childkey = uninstallkey.OpenSubKey(subkey);
							if (childkey != null)
							{
								foreach (var subchildkey in childkey.GetValueNames())
								{
									//Thread.Sleep(1);//test

									if (findtaskcts.IsCancellationRequested)
									{
										canceled = true;
										return;
									}

									var enexepath = Convert.ToString(childkey.GetValue(subchildkey));
									if (enexepath.Contains(softname))
									{
										this.Dispatcher.Invoke(new Action(() =>
										{
											enpath = System.IO.Path.GetDirectoryName(enexepath);
										}));
										complete = true;
										break;
									}
								}
							}
						}

					}
					catch
					{
						finish(2);
					}
					finally
					{
						if (complete | canceled)
						{
							finish(0);
						}
						else
						{
							finish(1);
						}

					}

					void finish(int finishcode)
					{
						//0成功，1未找到，2错误
						if (finishcode == 0)
						{
							this.Dispatcher.Invoke(new Action(() =>
							{
								enpathbox.Text = enpath;
								startbtntext.Text = "激活";
							}));
						}
						else if (finishcode == 1)
						{
							this.Dispatcher.Invoke(new Action(() =>
							{
								startbtntext.Text = "未自动找到希沃白板程序目录";
							}));
						}
						else if (finishcode == 2)
						{
							this.Dispatcher.Invoke(new Action(() =>
							{
								startbtntext.Text = "寻找希沃白板程序目录时发生错误";
							}));
						}
						this.Dispatcher.Invoke(new Action(() =>
						{
							// progress.IsIndeterminate = false;
						}));
					}
				}, ftct);

			};

			this.enpathbox.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				findtaskcts.Cancel();
			};
		}

		private void Window_ContentRendered(object sender, EventArgs e)
		{

		}

		private void startpatch_Click(object sender, RoutedEventArgs e)
		{
			if (enpathbox.Text == "" | new FileInfo(System.IO.Path.Combine(enpathbox.Text, "EasiNote.exe")).Exists == false)
			{
				if (MessageBox.Show("该希沃白板路径似乎不正确，是否继续", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
				{
					crack();
				}
			}
			else
			{
				crack();
			}
			void crack()
			{
				string targetDirectory = enpathbox.Text;
				string fileName = "Cvte.Platform.Basic.dll";
				string destPath = System.IO.Path.Combine(targetDirectory, fileName);

				try
				{
					string sourcePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Crackfiles", "Cvte.Platform.Basic.dll");
					if (!File.Exists(sourcePath))
					{
						// 回退方案：尝试相对路径
						sourcePath = "./Crackfiles/Cvte.Platform.Basic.dll";
					}
					
					FileInfo copyfileInfo = new FileInfo(sourcePath);

					// 确保目标文件夹存在 
					if (!System.IO.Directory.Exists(targetDirectory))
					{
						System.IO.Directory.CreateDirectory(targetDirectory);
					}

					// 执行拷贝 
					copyfileInfo.CopyTo(destPath, true);

					startbtntext.Text = "激活成功,即将自动退出";
					Task.Run(() =>
					{
						Thread.Sleep(1000);
						Environment.Exit(0);
					});
				}
				catch (System.IO.FileNotFoundException ex)
				{
					MessageBox.Show("找不到补丁文件 (Crackfiles/Cvte.Platform.Basic.dll)，请确保程序包完整。\n错误: " + ex.Message, "激活失败", MessageBoxButton.OK, MessageBoxImage.Error);
				}
				catch (System.IO.IOException ex)
				{
					// 通常是因为文件被占用 
					MessageBox.Show("文件正在使用中，请关闭相关程序后再试。\n原因: " + ex.Message, "激活失败", MessageBoxButton.OK, MessageBoxImage.Error);
				}
				catch (System.UnauthorizedAccessException)
				{
					// 权限问题 
					MessageBox.Show("没有权限写入该目录，请尝试以管理员身份运行此程序。", "激活失败", MessageBoxButton.OK, MessageBoxImage.Error);
				}
				catch (Exception ex)
				{
					MessageBox.Show("发生未知错误: " + ex.Message, "激活失败", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private void enpathbox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void browse_Click(object sender, RoutedEventArgs e)
		{
			// 创建CommonOpenFileDialog对象
			var enpathbd = new CommonOpenFileDialog();
			enpathbd.IsFolderPicker = true;

			// 显示文件夹选择对话框
			CommonFileDialogResult result = enpathbd.ShowDialog();

			if (result== CommonFileDialogResult.Ok)
			{
				enpathbox.Text= enpathbd.FileName;
			}
		}
	}
}
