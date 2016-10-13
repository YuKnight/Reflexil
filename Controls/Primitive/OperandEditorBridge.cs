﻿/* Reflexil Copyright (c) 2007-2016 Sebastien LEBRETON

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using System;

namespace Reflexil.Editors
{
	public class OperandEditorBridge<T> : IDisposable
	{
		private OperandEditor<T> _left;
		private OperandEditor<T> _right;

		private bool _recurseMarker;
		private readonly EventHandler _leftOnSelectedOperandChanged;
		private readonly EventHandler _rightOnSelectedOperandChanged;

		public OperandEditorBridge(OperandEditor<T> left, OperandEditor<T> right)
		{
			_left = left;
			_right = right;

			_leftOnSelectedOperandChanged = (sender, args) => OnSelectedOperandChanged(left, right);
			left.SelectedOperandChanged += _leftOnSelectedOperandChanged;

			_rightOnSelectedOperandChanged = (sender, args) => OnSelectedOperandChanged(right, left);
			right.SelectedOperandChanged += _rightOnSelectedOperandChanged;
		}


		private void OnSelectedOperandChanged(OperandEditor<T> source, OperandEditor<T> destination)
		{
			if (_recurseMarker)
				return;

			_recurseMarker = true;
			destination.SelectedOperand = source.SelectedOperand;
			_recurseMarker = false;
		}

		public void Dispose()
		{
			if (_left != null)
			{
				_left.SelectedOperandChanged -= _leftOnSelectedOperandChanged;
				_left = null;
			}

			if (_right == null)
				return;

			_right.SelectedOperandChanged -= _rightOnSelectedOperandChanged;
			_right = null;
		}
	}
}