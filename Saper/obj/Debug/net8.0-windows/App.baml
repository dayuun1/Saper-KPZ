   M S B A M L   `   `   `  ����@  <Saper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:SaperSaper  5      3clr-namespace:Saper.ConvertersSaper.Converters  5          	Saper.App   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      %localclr-namespace:Saper     5      1local1clr-namespace:Saper.Converters     5        �� 	Resources  5      .   (Saper.Converters.InverseBooleanConverter   InverseBooleanConverter  	BoolToVis  	IrpinType  ResourceButtonTemplate �� 
TargetType �� Property �� Value  
GameButton �� 
TargetType �� CornerRadius �� Padding ��Name �� Triggers	 P� Property
 P� Value3  -System.Windows.Media.Effects.DropShadowEffect   Color   	Direction   ShadowDepth   
BlurRadius   Opacity  
MenuButton �� BasedOn  UnavailableButton  ResourceBorder��%�
  &	        &	      &	 &     &	 f     7  &	 M    7  &	 M    7  &	 �    7  &	 <	      5   
   6)   �� 5	   
   6'   *�5
   
   ,(Fonts/IrpinType-Bold.ttf#Irpin Type Bold)�6Q   �� 5   
   " ��6/   .�5      ��  � ��$ 75��6&   �� 5       � ��$	 30 ��6'   �� 5       � ��$ 36(�6)   �� 5       �@��TextAlignment$ Center=�6.   �� 5       � ��8   6+   5      �� 5   
   " ��6   .�5      ��  � ��8   6+   �� 5       � ��$ 30(�6)   �� 5       � ��$	 350��6&   �� 5       � ��$ 90��6'   �� 5       � ��$ 
0, 0, 0, 5q�6'   �� 5       � �� �#F1ef426+   �� 5       �@�OverridesDefaultStyle$
 True��66   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� $ 20��6!   $��4q�63   $ 0q�6G   #��z��6S   ._�5      � .+�5   "   �� $ MyContentPresenter��63   #��z��5       $��Center=�5!      $��Center=�5"      5#      5$      5%      5'       5(      P� 5)      	 �@J�IsMouseOver$

 True��61   . �5*      ��  � �� �#CB74206E   �� 5+       � ��$	 330��6.   �� 5,       � ��$ 80��6/   �� 5-       � ��$ 0, 5, 0, 10q�6/   �� 5.       � ��$ 28(�61   �� 5/       �@J�Effect 50        51      $ #F1ef42��6/   $
 -400Q�6?   $ 8Q�6P   $ 30Q�6`   $	 0.5Q�6p   53      54      56      �� 57   
   " ��6   8   .�58      ��  � �� �#4c2c246+   �� 59       � �� 6(   �� *	GoCommand+5:      �� 5;   
   " ��6   .�5<      ��  � ��8   6+   �� 5=       �@�OverridesDefaultStyle$
 True��66   �� 5>       � ��$ 24(�6)   �� 5?       � ��$	 200��6&   �� 5@       � ��$ 70��6'   �� 5A       � �� �#4892CB6+   �� 5B       � �� 5C      �� 5D      " ��6&   .R�5E      �� $ 20��6!   $��4q�63   $ 0q�6G   #��z��6S   ._�5F      � .+�5G   "   �� $ MyContentPresenter��63   #��z��5H      $��Center=�5I      $��Center=�5J      5K      5L      5M      5O       5P      P� 5Q      	 � {�$
 False��6/   . �5R      ��  � �� �#4c2c2463   5S      P� 5T      	 �@J�IsMouseOver$

 True��61   . �5U      ��  �@J�Effect 5V        5W      $ Black��6/   $	 320Q�6=   $ 5Q�6M   $ 15Q�6]   $	 0.5Q�6m   5Y      5Z      5\      �� 5]   
   " ��6   .�5_   
   ��  � �� �#eedd8e6'   �� 5`   
    � ��$ 5q�6,   �� 5a   
    � �� �#bda55d6(   �� 5b   
    �@��CornerRadius$ 20��6)   �� 5c   
    � ��$ 0 0 4 0q�6#   5d      5e      5h      