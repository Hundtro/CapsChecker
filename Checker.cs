using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace CapsChecker
{	
	public class Checker : ApplicationContext
	{
		private bool status;
		private bool statusCurrect;
		
		private Thread checkThread;
		private NotifyIcon notification;
		
		public Checker()
		{
			notification = new NotifyIcon();
			
			notification.Visible = true;
			notification.Icon = SystemIcons.Shield;
			notification.Text = Settings.nName;
			notification.BalloonTipTitle = Settings.nTitle;
			
			notification.ContextMenuStrip = new ContextMenuStrip();
			notification.ContextMenuStrip.Items.Add(Settings.nMenuAbout, null, Menu_About);
			notification.ContextMenuStrip.Items.Add(Settings.nMenuExit, null, Menu_Exit);
			notification.MouseClick += Notify_Click;
			
			if(Control.IsKeyLocked(Keys.CapsLock))
			{
				statusCurrect = true;
				status = false;
				ShowNotification(true);
			}
			else
			{
				statusCurrect = false;
				status = true;
				ShowNotification(false);
			}
			
			checkThread = new Thread(CheckProcess);
			checkThread.Start();
		}
		
		void CheckProcess()
		{
			while(true)
			{
				statusCurrect = Control.IsKeyLocked(Keys.CapsLock);
				
				if(statusCurrect != status)
				{
					if(statusCurrect)
					{
						status = true;
						ShowNotification(true);
					}
					else
					{
						status = false;
						ShowNotification(false);
					}
				}
				
				Thread.Sleep(1000);
			}
		}
		
		void ShowNotification(bool isOn)
		{
			if(isOn)
				notification.BalloonTipText = Settings.nMessageOn;
			else
				notification.BalloonTipText = Settings.nMessageOff;
			
			notification.ShowBalloonTip(Settings.nWaitTime);
		}
		
		void Notify_Click(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
				notification.ContextMenuStrip.Show();
		}
		
		void Menu_About(object sender, EventArgs e)
		{
			MessageBox.Show("About!");
		}
		
		void Menu_Exit(object sender, EventArgs e)
		{
			checkThread.Abort();
			Application.Exit();
		}
	}
}
