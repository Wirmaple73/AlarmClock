   M S B A M L   `   `   `  ����   
AlarmClock    AlarmClock.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  زنگ هشدار��&   System.Windows.Media.TextOptions   TextFormattingMode$ Display=� � SizeToContent$ Width=�$��Tahoma)�
�������$	��450�� � 
ResizeMode$ CanMinimize=� � Icon$ icon_alarmclock.ico��.��� $��RightToLeft=�.+�o� $��25��$	��Top=�$	��517��$��Right=�.@�m� $��فایل��.@�m� -   $��خروج�� � ToolTip$  خروج از برنامه�� m� Icon �� $��Resources/icon_exit.png��$��22�� �� BitmapScalingMode$ HighQuality=�m� $��زنگ هشدار��.@�m� $ ��تغییر صدای زنگ�� �� $��Resources/icon_bell.png��$��22��.@�m� -    m�Name$&  MenuItem_SetDefaultSoundLocation��$��صدای پیش‌فرض��	 m� 	IsChecked	 . $9 3استفاده از صدای زنگ پیش‌فرض��m� -   $" MenuItem_ChangeSoundLocation��$%��انتخاب فایل صدا...��$J Dانتخاب فایل صوتی سفارشی برای صدای زنگ���� m� -   $��هشدار جدید��$$ ایجاد هشدار جدید�� �� $!��Resources/icon_addalarm.png��$��20��$ HighQuality=�m� -   $��حذف هشدار��$+ %حذف هشدار انتخاب شده�� �� $$��Resources/icon_deletealarm.png��$��20��$ HighQuality=�m� -   $��تغییر وضعیت��$E ?فعال/غیرفعال سازی هشدار انتخاب شده�� �� $#��Resources/icon_swapstatus.png��$��22��$ HighQuality=�m� $��راهنما��.@�m� -   $��درباره��$, &نمایش اطلاعات برنامه�� �� $��Resources/icon_help.png��$��23��$ HighQuality=��� -   
 ��Name$
 ListView_ScheduleList��$��
0,72,10,10q�$	��497��$
��Bold"� �� SelectionMode$ Single=�$��Right=� � 	Resources  �� 
TargetType �� Property �� Value��%/   '��       �� " ��.���  � ��$
 Left=� �� View �� $��RightToLeft=�.P��� $�� وضعیت�� �� Width$ 90�� �� DisplayMemberBinding �� *Status+�� $�� روز��$ 80�� �� *Day+�� $��	 زمان��$	 100�� �� *Time+�� $�� عنوان��$	 210�� �� *Title+�� -	   $
��Left=�$��	10,36,0,0q�$	��Top=�$	��105��$��28��$��13(�$$ ایجاد هشدار جدید��.���� .+��� $!��Resources/icon_addalarm.png��$��19��$��	0,-1,76,0q�$ HighQuality=��� $��هشدار جدید��$��
25,-19,0,0q��� -
   $
��Left=�$��
128,36,0,0q�$	��Top=�$	��105��$��28��$��13(�$+ %حذف هشدار انتخاب شده��.���� .+��� $$��Resources/icon_deletealarm.png��$��19��$��0,0,76,0q�$ HighQuality=��� $��حذف هشدار��$��
25,-19,0,0q��� -   $
��Left=�$��
246,36,0,0q�$	��Top=�$	��105��$��28��$��13(�$E ?فعال/غیرفعال سازی هشدار انتخاب شده��.���� .+��� $#��Resources/icon_swapstatus.png��$��21��$��	0,0,80,-1q�$ HighQuality=��� $��تغییر وضعیت��$��
25,-20,0,0q�