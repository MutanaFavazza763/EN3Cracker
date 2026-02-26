# <img width="64" height="64" alt="icon" src="https://github.com/user-attachments/assets/e09e40f5-e568-48be-b7c5-7ef584f25388" /> EN3Cracker - 希沃白板 3 激活工具

[![Build Status](https://github.com/MutanaFavazza763/EN3Cracker/actions/workflows/build.yml/badge.svg)](https://github.com/MutanaFavazza763/EN3Cracker/actions)
[![Release](https://img.shields.io/github/v/release/MutanaFavazza763/EN3Cracker)](https://github.com/MutanaFavazza763/EN3Cracker/releases)

解除希沃白板 3 (EasiNote 3) 试用期结束后的功能限制，恢复完整使用体验。

## 📖 项目背景

希沃白板 3 虽然已被官方停止维护，但其简洁与特定的功能模块在许多教学场景中仍具有不可替代性。由于官方激活服务器已关停，导致即使持有正版激活码的用户也无法完成验证。本项目通过逆向工程手段，修复了因验证服务器失效导致的无法激活问题。

## 🛠️ 工作原理

本工具通过替换希沃白板的核心激活组件 `Cvte.Platform.Basic.dll`，在软件调用激活验证接口时强制返回“已激活”状态，从而绕过服务器验证。

## ✨ 主要功能

- **自动化路径识别**：启动即自动搜索注册表，精确定位希沃白板安装目录。
- **一键激活**：简洁的交互设计，点击即可完成补丁替换。
- **健壮的错误处理**：能够识别并提示文件占用、权限不足或补丁缺失等异常情况。
- **极致精简**：无多余协议与弹窗，专注于核心功能。

## 🚀 使用说明

1. 在 [Releases](https://github.com/MutanaFavazza763/EN3Cracker/releases) 页面下载最新版本的压缩包并解压。
2. 运行 `EN3Cracker.exe`。
3. 程序会自动寻找希沃白板 3 的安装目录。如果未自动识别，请手动点击“浏览”选择。
4. 点击“激活”，等待提示成功后程序将自动退出。
5. 重新打开希沃白板 3，即可享受完整功能。

## 📸 运行效果

### 激活状态确认
激活后，关于页面将显示为已激活状态：
<img width="480" alt="image" src="https://github.com/user-attachments/assets/1e4b03d9-b946-4937-88f1-d47ca83b8164" />

### 功能恢复
诸如“手写识别”等依赖激活的功能可正常使用：
<img width="480" alt="image" src="https://github.com/user-attachments/assets/3e1c48e8-feb0131ba1" />

## ⚠️ 免责声明

- 本项目仅供学习交流逆向工程技术使用，严禁用于任何商业用途。
- 本仓库不提供希沃白板本体的源代码。
- 希沃白板 3 的著作权归希沃官方（广州视睿电子科技有限公司、广州视源电子科技股份有限公司）所有。
- 请在下载后的 24 小时内删除。如果您喜欢该软件，请支持官方后续的正版产品（如希沃白板 5）。
