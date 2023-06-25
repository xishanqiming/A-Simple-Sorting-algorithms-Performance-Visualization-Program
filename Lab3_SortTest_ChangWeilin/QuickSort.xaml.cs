﻿/**QuickSort.xaml.cs
*@author: CHANG WEI-LIN
*@Last Modify: 4/9/2023
*@About: This program is the window of the QuickSort Algorithms Test Application With C# WPF.
*/

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Wpf;
using System.Windows.Input;
using OxyPlot.Axes;

namespace Lab3_SortTest_ChangWeilin
{
    //QuickSort窗口类
    public partial class QuickSort : Window
    {
        //设置实例字段，_filePath 标记选定文本的绝对路径，_data 数组用于存取数据，_dataSize 用于设置用户选定的数据规模
        private string _filePath;
        private int _dataSize;
        private double[] _data;
        private double lg_dataSize;
        private double ttime;

        //_filePath（选定文件的路径），_dataSize（用户选择的数据大小），_data（用于存储数据的数组），lg_dataSize（数据大小的对数），ttime（总排序时间）
        //OxyPlot绘图所需的对象：_plotModel（绘图模型），_lineSeries（线条系列），_lastMousePosition（记录上次鼠标位置）
        private readonly PlotModel _plotModel;
        private readonly LineSeries _lineSeries;
        private ScreenPoint _lastMousePosition;

        //构造函数，初始化各组件，并设置OxyPlot图表的基本属性
        public QuickSort()
        {
            InitializeComponent();
            cbDataSelection.SelectedIndex = 0;

            _plotModel = new PlotModel
            {
                IsLegendVisible = false,
                PlotAreaBorderColor = OxyColors.Transparent
            };

            _lineSeries = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                MarkerStroke = OxyColors.Black
            };

            _plotModel.Series.Add(_lineSeries);
            _plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Data Size (lg)", Maximum = 10 });
            _plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Total Milliseconds" });
            plotView.Model = _plotModel;
        }

        //初始化读入文件按钮响应函数，准许读入文本格式的文件，并将文件路径显示在界面上
        private void ReadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            //显示提示信息：读入文本的绝对路径
            if (openFileDialog.ShowDialog() == true)
            {
                _filePath = openFileDialog.FileName;
                btnReadFile.Content = "文件路径";
                txtInformation.Text = $"File: {_filePath}";
            }
        }

        //初始化选定数据规模按钮响应函数
        private void DataSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] dataOptions = { "10", "100", "1000", "10000", "100000", "1000000", "all" };
            string selectedDataOption = dataOptions[cbDataSelection.SelectedIndex];

            //若选择全部数据，则不设置最大数据上限（设定为-1）
            if (selectedDataOption == "all")
            {
                _dataSize = -1;
            }
            else
            {
                _dataSize = int.Parse(selectedDataOption);
            }
            //显示提示信息：选择的数据规模
            txtInformation.Text = $"选定排序的数据规模: {selectedDataOption}";
        }

        //初始化开始按钮响应函数
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            //提示错误信息：未选择合法的读入文件
            if (_filePath == null)
            {
                MessageBox.Show("请选择一个读入文件！", "读入文件尚未选择", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //读入选定文件的数据，以空格' '，作为每个数据的分隔符，加入待排数组中
            try
            {
                string fileContent = File.ReadAllText(_filePath);
                _data = fileContent.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                   .Select(double.Parse)
                                   .ToArray();

                //根据用户设定的数据规模进行限制
                //只会读入限定规模的数据
                if (_dataSize != -1 && _data.Length > _dataSize)
                    _data = _data.Take(_dataSize).ToArray();

                else
                    _data = _data.ToArray();

                //数据全部就绪后（也就是读入数据完毕后），开始对快速排序的时间计时
                QuickSortAlgorithm(_data, 0, _data.Length - 1, out TimeSpan sortingTime);
                txtInformation.Text = $"快速排序时间为： {sortingTime.TotalMilliseconds} 毫秒";

                //将排序完成的数据以文本文件格式输出到原读入文件同目录下，编码格式为 UTF-8，每个数据之间空一个空格（仍为原格式）
                string outputPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(_filePath), "sorted_output.txt");
                File.WriteAllText(outputPath, string.Join(" ", _data.Select(i => i.ToString())) + " ", Encoding.UTF8);
                double sortingTimeMs = sortingTime.TotalMilliseconds;

                MessageBox.Show($"排序成功！总耗时为：{sortingTime.TotalMilliseconds} 毫秒。排序结果已保存至：{outputPath}", "排序已结束", MessageBoxButton.OK, MessageBoxImage.Information);
                _filePath = null;
                ttime = sortingTime.TotalMilliseconds;
                lg_dataSize = Math.Log(_data.Length, 10);

                AddPointToLineSeries(_lineSeries, new DataPoint(lg_dataSize, ttime));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //向LineSeries添加点，根据x值从小到大排序连成折线
        private void AddPointToLineSeries(LineSeries lineSeries, DataPoint newPoint)
        {
            int index = 0;
            bool added = false;
            for (; index < lineSeries.Points.Count; index++)
            {
                if (newPoint.X < lineSeries.Points[index].X)
                {
                    lineSeries.Points.Insert(index, newPoint);
                    added = true;
                    break;
                }
            }
            //如果新添加的点的横坐标是最大值，则将其添加到末尾
            if (!added)
                lineSeries.Points.Add(newPoint);
            _plotModel.InvalidatePlot(true);
        }

        //快速排序的核心算法，输入一个double类型数组，传入四个参数：数据数组，起始索引，结束索引，排序时间
        //输出排好序的数据
        private void QuickSortAlgorithm(double[] data, int left, int right, out TimeSpan sortingTime)
        {
            //创建计数器，开始计时
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //调用快速排序的递归算法
            _QuickSort(data, left, right);

            //递归调用完成后，停止计时，返回排序所耗时间
            stopwatch.Stop();
            sortingTime = stopwatch.Elapsed;
        }

        //快速排序的递归算法
        //每次都将当前将待排序列分为两部分，递归进行，分治排序
        private void _QuickSort(double[] data, int left, int right)
        {
            if (left < right)
            {    //当左边界小于右边界时进行排序，获取基准值所在的索引
                int pivotIndex = Partition(data, left, right);
                //对左半部分进行快速排序
                _QuickSort(data, left, pivotIndex - 1);
                //对右半部分进行快速排序
                _QuickSort(data, pivotIndex + 1, right);
            }
        }

        //基准值分区（Partition）函数
        private int Partition(double[] data, int left, int right)
        {
            //选择右边界的值作为当前一次快速排序的基准值
            double pivot = data[right];
            //初始化 i 为左边界减 1
            int i = left - 1;

            //遍历左边界到右边界之间的数据（不包含边界处的值）
            for (int j = left; j < right; j++)
            {
                if (data[j] <= pivot)
                {
                    i++;
                    //交换 data[i] 和 data[j] 的值
                    (data[i], data[j]) = (data[j], data[i]);
                }
            }

            //交换 data[i + 1] 和 data[right] 的值
            (data[i + 1], data[right]) = (data[right], data[i + 1]);

            //返回新的基准值的索引
            return i + 1;
        }

        private void TxtData_TextChanged(object sender, TextChangedEventArgs e)
        {
            //啥也没有
        }

        //界面交互的事件处理函数，用于处理鼠标在OxyPlot图表上的操作
        private void PlotView_MouseMove(object sender, MouseEventArgs e)
        {
            //获取当前鼠标位置并将其转换为OxyPlot的ScreenPoint类型
            var currentPosition = ToScreenPoint(e.GetPosition(plotView));

            //如果按下鼠标左键
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //如果_lastMousePosition不是默认值（即有上一个鼠标位置）
                if (!_lastMousePosition.Equals(default))
                {
                    //计算鼠标在X轴和Y轴上的移动距离
                    var deltaX = currentPosition.X - _lastMousePosition.X;
                    var deltaY = currentPosition.Y - _lastMousePosition.Y;

                    //根据鼠标在X轴和Y轴上的移动距离移动图表的X轴和Y轴
                    _plotModel.DefaultXAxis.Pan(-deltaX);
                    _plotModel.DefaultYAxis.Pan(-deltaY);

                    //更新图表
                    _plotModel.InvalidatePlot(false);
                }
            }
        }

        //将WPF的Point类型转换为OxyPlot的ScreenPoint类型
        private static ScreenPoint ToScreenPoint(Point point)
        {
            return new ScreenPoint(point.X, point.Y);
        }
    }
}

/* Program Over */