   M S B A M L   `   `   `  ���� L  HEloBuddy.Loader, Version=0.1.14.12, Culture=neutral, PublicKeyToken=nullC&clr-namespace:EloBuddy.Loader.ControlsEloBuddy.Loader.Controls  A%clr-namespace:EloBuddy.Loader.GlobalsEloBuddy.Loader.Globals  E'clr-namespace:EloBuddy.Loader.ConverterEloBuddy.Loader.Converter  1    +EloBuddy.Loader.Views.FirstTimeWizardWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ;controls&clr-namespace:EloBuddy.Loader.Controls     9globals%clr-namespace:EloBuddy.Loader.Globals     =	converter'clr-namespace:EloBuddy.Loader.Converter     $	��700��$	��950��  � 
ResizeMode$  NoResize=� � WindowStartupLocation$ CenterScreen=�$��1q� � WindowStyle$
 None=�
���   ����	#FFFDFDFD � Icon$6 0/EloBuddy.Loader;component/resources/favicon.ico�� � 	Resources +   %EloBuddy.Loader.Globals.MultiLanguage   MultiLanguage=  7EloBuddy.Loader.Converter.WindowTitleRandomizeConverter  WindowTitleConverter<   6EloBuddy.Loader.Converter.BitmapToImageSourceConverter   BitmapToImageSourceConverter��%-   &	        &	      &	 
            � Title ��  �� 	Converter# [ .���  � RowDefinitions �� $��39 ��� .+�� -   $��0��$	��Top=����	#FF40A6D1$��40��$��2�� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect  	   
BlurRadius$	 10Q�
   ShadowDepth$
 3Q�   	Direction$	 270Q�   Opacity$	 0.2Q�.+�/   )EloBuddy.Loader.Controls.HeaderDecoration  $��0,0,1,0q��� $��20(�$��SemiBold"����#FAFAFA$��11,0,0,0q�����  �� Source# [   �� Path$ Text.TitleFirstTimeWizard��� $��Center=� �� Orientation$ 
Horizontal=�$��0,0,1,0q�$��Right=� J� RenderTransformOrigin$ -0.275,0.575&�.+��� -    ��Name$ CloseButton��$��40��$��40��$	����  CloseMinMaxButtonStyle#���  
������ ���	#FF40A6D1
�������$.��(pack://application:,,,/font/#FontAwesome)�$
��20px(�$��Bottom=�� $��1�� �� $��0 ��� $��* ��� $
��Auto � � ColumnDefinitions �� $��0 ��� $��* �.+��� $��0,5,0,0q����	#FFF7F7F7$��0��$��0��$��2��._�0   *EloBuddy.Loader.Controls.WizardProgressBar  ���	#FF086398 J� SnapsToDevicePixels . $
��Left=�$
��Auto������ *Steps+   Progress �� *
Progress+ � LayoutTransform ��  �� Angle$ 90Q��� $��1��$��1��$��1q�$��20 30 20 10q����	#FFF7F7F7���� -   $��
TitleLabel��$��Title��$��20(�$��SemiBold"����	#FF40A6D1.���� $ Vertical=�.+��� $��0q�$��3 0 0 0q�
���iii�$��50��.���� -   $��ContentLabel��$��Description��$��13(�$
��Bold"��� $��0q�$��3 0 0 0q�
���iii�$��5���� $	��800��$	��400������ *PreviewImage+# [ d� -   $��ToggleButton��  AnimatedSwitch#���  $��30��$
��Left=�$��	5 50 30 0q��� $��Bottom=�$��0,0,0,0q�$��40�����	#FFF7F7F7$��2��$��0��$��2��._��� $��Center=�$��Right=�$��0 0 10 0q�$ 
Horizontal=�.+��� -   $ 
SkipButton��  RoundedSettingsButtonStyle#���  $
��Bold"����	#FF40A6D1$
��25 4q����	#FFF7F7F7���	#FF40A6D1$��1q����� # [  $ Text.ButtonSkip��� -   $ 
NextButton��$��10 0 0 0q�#���  $
��Bold"�
�������$
��25 4q����	#FF40A6D1���	#FF40A6D1$��1q����� # [  $ Text.ButtonNext�