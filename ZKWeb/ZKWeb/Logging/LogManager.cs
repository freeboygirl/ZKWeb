﻿using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using ZKWeb.Server;

namespace ZKWeb.Logging {
	/// <summary>
	/// Log manager
	/// </summary>
	public class LogManager {
		/// <summary>
		/// Log message to file
		/// </summary>
		public virtual void Log(string filename, string message) {
			// Create log directory
			var pathConfig = Application.Ioc.Resolve<PathConfig>();
			var logsDirectory = pathConfig.LogsDirectory;
			Directory.CreateDirectory(logsDirectory);
			// Write to console
			Console.Write(message);
			// Write to log file
			// Retry up to 100 times if inconsistency occurs between the threads
			var now = DateTime.UtcNow.ToLocalTime();
			var path = Path.Combine(logsDirectory, filename);
			for (int n = 0; n < 100; ++n) {
				try {
					File.AppendAllText(path, message, Encoding.UTF8);
					break;
				} catch (IOException) {
					Thread.Sleep(5);
				}
			}
		}

		/// <summary>
		/// Log debug level message
		/// </summary>
		public virtual void LogDebug(string message,
			[CallerMemberName] string memberName = null) {
			var now = DateTime.UtcNow.ToLocalTime();
			var filename = $"Debug.{now.ToString("yyyyMMdd")}.log";
			Log(filename, $"{now.ToString()} ({memberName}) {message}\r\n");
		}

		/// <summary>
		/// Log information level message
		/// </summary>
		public virtual void LogInfo(string message,
			[CallerMemberName] string memberName = null) {
			var now = DateTime.UtcNow.ToLocalTime();
			var filename = $"Info.{now.ToString("yyyyMMdd")}.log";
			Log(filename, $"{now.ToString()} ({memberName}) {message}\r\n");
		}

		/// <summary>
		/// Log error level message
		/// </summary>
		public virtual void LogError(string message,
			[CallerMemberName] string memberName = null,
			[CallerFilePath] string filePath = null,
			[CallerLineNumber] int lineNumber = 0) {
			var now = DateTime.UtcNow.ToLocalTime();
			var filename = $"Error.{now.ToString("yyyyMMdd")}.log";
			Log(filename, $"{now.ToString()} ({filePath}:{lineNumber} {memberName}) {message}\r\n");
		}

		/// <summary>
		/// Log transaction message,
		/// It's for the things releated to money
		/// </summary>
		public virtual void LogTransaction(string message,
			[CallerMemberName] string memberName = null) {
			var now = DateTime.UtcNow.ToLocalTime();
			var filename = $"Transaction.{now.ToString("yyyyMMdd")}.log";
			Log(filename, $"{now.ToString()} ({memberName}) {message}\r\n");
		}
	}
}
