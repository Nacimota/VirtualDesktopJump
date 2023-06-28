using Microsoft.Win32;
using Nacimota.VDJ.Properties;
using Nacimota.Win32;

namespace Nacimota.VDJ;
internal class VdjAppContext : ApplicationContext
{
    private NotifyIcon leftIcon;
    private NotifyIcon rightIcon;
    private ContextMenuStrip contextMenu;

    public VdjAppContext()
    {
        Application.ApplicationExit += Application_ApplicationExit;

        bool LightTaskbarTheme = IsTaskbarUsingLightTheme();

        leftIcon = new() { Visible=true, Icon = LightTaskbarTheme ? Resources.DarkLeftArrowIcon : Resources.LightLeftArrowIcon };
        rightIcon = new() { Visible=true, Icon = LightTaskbarTheme ? Resources.DarkRightArrowIcon : Resources.LightRightArrowIcon };

        leftIcon.MouseClick += LeftIcon_Click;
        rightIcon.MouseClick += RightIcon_Click;

        contextMenu = new () { ShowImageMargin = false };
        var exitButton = new ToolStripMenuItem("Exit");
        exitButton.Click += ExitButton_Click;

        contextMenu.Items.Add(new ToolStripLabel("VirtualDesktopJump") { Font = new Font(contextMenu.Font, FontStyle.Bold) });
        contextMenu.Items.Add(new ToolStripSeparator());
        contextMenu.Items.Add(exitButton);

        leftIcon.ContextMenuStrip = contextMenu;
        rightIcon.ContextMenuStrip = contextMenu;
    }

    private void LeftIcon_Click(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left) Winuser.SendCtrlWinLeft();
    }

    private void RightIcon_Click(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left) Winuser.SendCtrlWinRight();
    }

    private void ExitButton_Click(object? sender, EventArgs e) => Application.Exit();

    private bool IsTaskbarUsingLightTheme()
    {
        try
        {
            if (Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", null) is int regValue && regValue != 0)
            {
                return true;
            }
        }
        catch (Exception ex) when (ex is IOException or System.Security.SecurityException)
        {
            // I'll take "things that shouldn't happen" for $500 please, Alex
        }

        return false;
    }

    private void Application_ApplicationExit(object? sender, EventArgs e)
    {
        leftIcon.Visible = false;
        rightIcon.Visible = false;
        leftIcon.Dispose();
        rightIcon.Dispose();
    }
}
