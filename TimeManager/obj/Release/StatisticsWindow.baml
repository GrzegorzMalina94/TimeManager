   M S B A M L   `   `   `  ���� F  BTimeManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:TimeManagerTimeManager  "    TimeManager.StatisticsWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:TimeManager       � Title$  StatisticsWindow�����#cccccc$	��600��$
��1200��.���  � RowDefinitions �� $
��2.5* ��� $	��10* ��� $��17.5* ��� $��67.5* ��� $
��2.5* � � ColumnDefinitions �� $
��2.5* ��� $	��95* ��� $
��2.5* �.+��� -   $��Title��$��1��$��1��$��Center=�$��Center=�$��32(�$��Times New Roman)�.��
Statistics� -   $��ListBoxesGrid��$��1��$��2�� �� $��1* ��� $��1* ��� $��1* ��� $��1* �.+��� .+���    UpprStatWindLabl#��[  .��Month�� -   $��MonthComboBox��$��Center=�$��Center=�$	��120��.@��� .��	January�� .��
February�� .��March�� .��April�� .��May�� .��June�� .��July�� .��August�� .��	September�� .��	October�� .��
November�� .��
December�� $��1��.+��� #��[  .��Week�� -   $��StatWindWeekComboBox��$��Center=�$��Center=�$	��120���� $��2��.+��� #��[  .��Day�� -   $��DayComboBox��$��Center=�$��Center=�$	��120��.@��� .��Monday�� .��	Tuesday�� .��	Wednesday�� .��
Thursday�� .��Friday�� .��
Saturday�� .��Sunday�� $��3��.+��� #��[  .��Period�� -   $��PeriodComboBox��$��Center=�$��3��$��Center=�$	��120��.@��� .��Day�� .��Week�� .��Month�� 
�������$��3��$��1��$
��Auto��.��� -   $��InfoGrid��$��1��$��3�� �� $��2* ��� $��1* ��� $��1* ��� $��1* ��� $��2* �.+��� -   $��ActivityName��$��0��.+���   LowrStatWindLabl#��[ $
��35,0q�$
��Left=�$��Activity��$
��Bold"��� -	   $��QrtsQnttyPlnnd��$��1��.+��� #��[ $)��#Quarters quantity
according to plan��$
��Bold"��� -
   $��QrtsQnttyRlty��$��2��.+��� #��[ $)��#Quarters quantity
       in reality��$
��Bold"��� -   $��RltyToPlanSP��$��3��.+��� #��[ $"��Reality / plan
  in percents��$
��Bold"�