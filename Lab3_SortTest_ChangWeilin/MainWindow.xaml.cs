/**MainWindow.xaml.cs
*@author: CHANG WEI-LIN
*@Last Modify: 4/9/2023
*@About: This program is the main window of this Sort Algorithms Test Application with C# WPF.
*/

using System.Windows;
using System.Windows.Input;

namespace Lab3_SortTest_ChangWeilin
{
    //Interaction logic for MainWindow.xaml
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //按钮一：链接到“HeapSort 堆排序”窗体
        private void Button_HeapClick(object sender, RoutedEventArgs e)
        {
            //生成新的主窗体
            HeapSort HeapSort = new HeapSort
            {
                Title = "Heap Sort"
            };

            //设置系统主窗体
            App.Current.MainWindow = HeapSort;
            HeapSort.Owner = this;

            // 显示新窗口为模式对话框
            _ = HeapSort.ShowDialog();
        }

        //按钮二：链接到“QuickSort 快速排序”窗体
        private void Button_QuickClick(object sender, RoutedEventArgs e)
        {
            //生成新的主窗体
            QuickSort QuickSort = new QuickSort
            {
                Title = "Quick Sort"
            };

            //设置系统主窗体
            App.Current.MainWindow = QuickSort;
            QuickSort.Owner = this;

            // 显示新窗口为模式对话框
            _ = QuickSort.ShowDialog();
        }

        //按钮三：链接到“Bubble Sort”窗体
        private void Button_BubbleClick(object sender, RoutedEventArgs e)
        {
            //生成新的主窗体
            BubbleSort BubbleSort = new BubbleSort
            {
                Title = "Bubble Sort"
            };

            //设置系统主窗体
            App.Current.MainWindow = BubbleSort;
            BubbleSort.Owner = this;

            // 显示新窗口为模式对话框
            _ = BubbleSort.ShowDialog();
        }

        //用户点击按钮一 Heap Sort 时消息栏的反馈
        private void Button_HeapClick_MouseEnter(object sender, MouseEventArgs e)
        {
            StatusBarText.Text = " Heap Sort 堆排序，通过构建一个最大堆，进行排序。 ";
        }

        //用户光标移开后消息栏的反馈
        private void Button_HeapClick_MouseLeave(object sender, MouseEventArgs e)
        {
            StatusBarText.Text = string.Empty;
        }

        //用户点击按钮二 Quick Sort 时消息栏的反馈
        private void Button_QuickClick_MouseEnter(object sender, MouseEventArgs e)
        {
            StatusBarText.Text = " Quick Sort 快速排序，选定基准，将序列分为两部分。 ";
        }

        //用户光标移开后消息栏的反馈
        private void Button_QuickClick_MouseLeave(object sender, MouseEventArgs e)
        {
            StatusBarText.Text = string.Empty;
        }

        //用户点击按钮三 Bubble Sort 时消息栏的反馈
        private void Button_BubbleClick_MouseEnter(object sender, MouseEventArgs e)
        {
            StatusBarText.Text = " Bubble Sort 冒泡排序，每趟排序都返回当前序列的最小值。 ";
        }

        //用户光标移开后消息栏的反馈
        private void Button_BubbleClick_MouseLeave(object sender, MouseEventArgs e)
        {
            StatusBarText.Text = string.Empty;
        }

        //用户点击作者信息时消息栏的反馈
        private void PerPho_MouseEnter(object sender, MouseEventArgs e)
        {
            StatusBarText.Text = " 绿色与自由软件倡导者同盟，CHANG WEI-LIN 制作。";
        }

        //用户光标移开后消息栏的反馈
        private void PerPho_MouseLeave(object sender, MouseEventArgs e)
        {
            StatusBarText.Text = string.Empty;
        }
    }
}

/* Program Over */