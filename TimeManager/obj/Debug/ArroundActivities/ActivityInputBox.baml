   M S B A M L   `   `   `  ����F  BTimeManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:TimeManagerTimeManager  5   	   "    TimeManager.ActivityInputBox   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:TimeManager     5   	   -   5          � Title$  Activity Input Box��5   	    � SizeToContent$ WidthAndHeight=�6$    � WindowStartupLocation$ CenterScreen=�6C   .��5
      �  � RowDefinitions 5   
   �� 5      �� 5      �� 5      �� 5      �� 5      .+�5   
   �� $+��%Please enter the name of an activity:��6   $��0��6@   $��15q�6M   �� 5   
   -   5        $��ActivityName��5      $��Activity��6'   $��
20,0,15,15q�67   $��1��6K   �� 5   
   $+��%Please choose color for the activity:��6   $��2��6@   $��
15,0,15,15q�6M   � 5   
   -   5        $��
ColorsGrid��5      $��3��6!    5      �� 5      �� 5      �� 5       � ColumnDefinitions 5      �� 5      �� 5      �� 5      �� 5      �� 5       �� 5!      5#      �� 5$   
    �� IsCancel . $��4��6!   $��Cancel��6.   $��50��6?   $��Right=�6J   $��0,0,10,5q�6f   �� 5%   
   -   5        $��ApplyButton��5%       �� 	IsDefault . $��4��66   $��Apply��6C   $��50��6S   $��Right=�6^   $��0,0,70,5q�6z   5&      5'      5(      