   M S B A M L   `   `   `  ����C  ?Checkers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:CheckersCheckers  5   	       Checkers.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Checkers     5   	   -   5          �Name$  Checkers��5
   	    � Title$ 
MainWindow��5   	   $	��800��6    � SizeToContent$ Height=�6(   ���#181818 � 	Resources 5         NoMouseOverButtonTemplate �� 
TargetType �� Triggers P� Property P� Value �� Value	 �� Property��%2  &	        �� 5   
   " ��5      .R�5      �� #��z��6   #��z��5      #��z��5      ._�5      �� #��z��6#   #��z��5      5       5      P� 5       � {�$ False��5      . �5      �� # Z�5      	 � ���� 5      # Z
�5      	 � ��5      5      5       .��5"      � 
 � ColumnDefinitions
 5$   
   �� 5%      $��* �6   $	��100��6)   �� 5&      $��8* �6   .+�5)   
   �� $��0��6   ���#1212126%   � 5+   
   -   5         �Name$
 Menu��5+      $��0��6    � RowDefinitions 5,      �� 5-      $
��Auto �6    �� 5.      $
��Auto �6    �� 5/      $
��Auto �6    .+�52      �� -   5         ��Name$ ResetButton��52      $��70��6*   $	��700��69   
����������#222222$��8q�54   /   .��6:   
Reset Game55      �� 57      -   5        $ InvertBoard��57      $��1��6*   $��70��67   $	��700��6F   ���#222222
�������$��8q�59   /   .��6:   Invert Board5:      5<      (� 5>      $��1��6    (� Stretch$ Uniform=�6   .��5?      � -   5        $ CheckerBoard��5?      $��1��6)   $	��800��66   $	��800��6B   ��5@      �� 5A       �� ImageSource$ Assets\Board.png��6!    5D      �� 5E      $��4* �6$   �� 5F      $��2* �6$   �� 5G      $��4* �6$   
 5J      �� 5K      $��5* �6'   �� 5L      $��6* �6'   �� 5M      $��5* �6'   .+�5P      A� -   5         A�Name$ MoveHighlights��5P       A� Rows$ 8��66    A� Columns$ 8��6?   $��3��5Q      $��3��62   5R      A� 5T      -   5        $ 	PieceGrid��5T      $ 8��61   $ 8��6:   $��3��5U      $��3��62    �� BitmapScalingMode$ HighQuality=�5V      5W      �� 5Y      -   5         ��Name$ WinnerPopUp��5Y      $��1��6.   $��1��6;   ���#5E1914 J� Opacity$ 0Q�5Z   .    �� CornerRadius$ 10��5[      ���#420D09$��5q�5\      ._�5^      �� -	   5         ��Name$ WinnerPopUpText��5^       $��
White Wins��69   $��Center=�5_       $��Center=�5`       $��40(�5a        �� TextAlignment$ Center=�6.   $
��Bold"�5b       
�������62   5c      5d      5e      5i      5j      