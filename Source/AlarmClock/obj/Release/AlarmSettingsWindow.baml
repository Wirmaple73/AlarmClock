   M S B A M L   `   `   `  ����   
AlarmClock$    AlarmClock.AlarmSettingsWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  تنظیمات هشدار��$	��400��$	��300�� � 
ResizeMode$ NoResize=� � ShowInTaskbar .  &   System.Windows.Media.TextOptions   TextFormattingMode$ Display=�$��Tahoma)�
������� � Icon$! Resources/icon_addalarm.png��.��� $��RightToLeft=�.+��� $��	10,8,10,0q�$��تنظیمات��$	��301��$	��Top=�.��� .+��� $��وضعیت:��$
��Left=�$��5,13,0,0q�$	��Top=��� $��	زمان:��$
��Left=�$��5,91,0,0q�$	��Top=��� $��عنوان:��$
��Left=�$��	5,127,0,0q�$	��Top=��� $��روز:��$
��Left=�$��5,49,0,0q�$	��Top=�	� -    	�Name$ RadioButton_Enabled��$��فعال��$
��Left=�$��	65,20,0,0q�$	��Top=� d� 	IsChecked$
 TrueY� � ToolTip$4 .هشدار، فعال ایجاد می‌شود��	� $��غیرفعال��$
��Left=�$��
135,20,0,0q�$	��Top=�$: 4هشدار، غیرفعال ایجاد می‌شود���� -    ��Name$ ComboBox_Day��$
��Left=�$��	65,53,0,0q�$	��Top=�$��93��$" انتخاب روز هفته��.@��� $��یکشنبه���� $��دوشنبه���� $��سه‌شنبه���� $��چهارشنبه���� $��پنجشنبه���� $��جمعه���� $��شنبه���� $��
دقیقه��$
��Left=�$��	68,75,0,0q�$	��Top=�$��10(��� $��ساعت��$
��Left=�$��
120,75,0,0q�$	��Top=�$��10(��� -   $ ComboBox_Hour��$
��Left=�$��
119,95,0,0q�$	��Top=�$��40��$&  انتخاب زمان - ساعت���� -   $ ComboBox_Minute��$
��Left=�$��	65,95,0,0q�$	��Top=�$��40��$( "انتخاب زمان - دقیقه���� $��:��$
��Left=�$��
105,91,0,0q�$	��Top=��� -   	 ��Name$	 TextBox_Title��$
��Left=�$	��100��$��	6,156,0,0q�
 �� TextWrapping$

 Wrap=�$	��Top=�$	��240��" �� VerticalScrollBarVisibility$
 Auto=� �� 
AcceptsTab .  �� 	MaxLength$	 512��$&  عنوان (دلیل) هشدار���� -   $��ذخیره هشدار��$
��Left=�$��
19,323,0,0q�$	��Top=�$	��105��$��28��$��13(� �� 	IsDefault . $= 7ذخیره هشدار با تنظیمات ذکر شده���� -   $��بازگشت��$
��Left=�$��137,323,0,0q�$	��Top=�$��77��$��28��$��13(� �� IsCancel . $K Eبازگشت به پنجره اصلی و عدم ذخیره هشدار��