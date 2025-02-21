﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information
namespace DotNetNuke.Services.Installer.Log
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Instrumentation;

    /// <summary>The Logger class provides an Installer Log.</summary>
    public class Logger
    {
        private static readonly ILog DnnLogger = LoggerSource.Instance.GetLogger(typeof(Logger));
        private readonly IList<LogEntry> logs;
        private string errorClass;
        private bool hasWarnings;
        private string highlightClass;
        private string normalClass;
        private bool valid;

        /// <summary>Initializes a new instance of the <see cref="Logger"/> class.</summary>
        public Logger()
        {
            this.logs = new List<LogEntry>();

            this.valid = true;
            this.hasWarnings = Null.NullBoolean;
        }

        public bool HasWarnings
        {
            get
            {
                return this.hasWarnings;
            }
        }

        /// <summary>Gets a List of Log Entries.</summary>
        public IList<LogEntry> Logs
        {
            get
            {
                return this.logs;
            }
        }

        /// <summary>Gets a value indicating whether the Installation was Valid.</summary>
        public bool Valid
        {
            get
            {
                return this.valid;
            }
        }

        /// <summary>Gets or sets the CSS class used for Error Log Entries.</summary>
        public string ErrorClass
        {
            get
            {
                if (string.IsNullOrEmpty(this.errorClass))
                {
                    this.errorClass = "NormalRed";
                }

                return this.errorClass;
            }

            set
            {
                this.errorClass = value;
            }
        }

        /// <summary>Gets or sets the CSS class used for Log Entries that should be highlighted.</summary>
        public string HighlightClass
        {
            get
            {
                if (string.IsNullOrEmpty(this.highlightClass))
                {
                    this.highlightClass = "NormalBold";
                }

                return this.highlightClass;
            }

            set
            {
                this.highlightClass = value;
            }
        }

        /// <summary>Gets or sets the CSS class used for normal Log Entries.</summary>
        public string NormalClass
        {
            get
            {
                if (string.IsNullOrEmpty(this.normalClass))
                {
                    this.normalClass = "Normal";
                }

                return this.normalClass;
            }

            set
            {
                this.normalClass = value;
            }
        }

        /// <summary>The AddFailure method adds a new LogEntry of type Failure to the Logs collection.</summary>
        /// <remarks>This method also sets the Valid flag to false.</remarks>
        /// <param name="failure">The description of the LogEntry.</param>
        public void AddFailure(string failure)
        {
            this.logs.Add(new LogEntry(LogType.Failure, failure));
            DnnLogger.Error(failure);
            this.valid = false;
        }

        public void AddFailure(Exception ex)
        {
            this.AddFailure(Util.EXCEPTION + ex);
            Exceptions.Exceptions.LogException(ex);
        }

        /// <summary>The AddInfo method adds a new LogEntry of type Info to the Logs collection.</summary>
        /// <param name="info">The description of the LogEntry.</param>
        public void AddInfo(string info)
        {
            this.logs.Add(new LogEntry(LogType.Info, info));
            DnnLogger.Info(info);
        }

        /// <summary>The AddWarning method adds a new LogEntry of type Warning to the Logs collection.</summary>
        /// <param name="warning">The description of the LogEntry.</param>
        public void AddWarning(string warning)
        {
            this.logs.Add(new LogEntry(LogType.Warning, warning));
            DnnLogger.Warn(warning);
            this.hasWarnings = true;
        }

        /// <summary>The EndJob method adds a new LogEntry of type EndJob to the Logs collection.</summary>
        /// <param name="job">The description of the LogEntry.</param>
        public void EndJob(string job)
        {
            this.logs.Add(new LogEntry(LogType.EndJob, job));
            DnnLogger.Info(job);
        }

        /// <summary>GetLogsTable formats log entries in an HtmlTable.</summary>
        /// <returns>An <see cref="HtmlTable"/> instance.</returns>
        public HtmlTable GetLogsTable()
        {
            var arrayTable = new HtmlTable();
            foreach (LogEntry entry in this.Logs)
            {
                var tr = new HtmlTableRow();
                var tdType = new HtmlTableCell();
                tdType.InnerText = Util.GetLocalizedString("LOG.PALogger." + entry.Type);
                var tdDescription = new HtmlTableCell();
                tdDescription.InnerText = entry.Description;
                tr.Cells.Add(tdType);
                tr.Cells.Add(tdDescription);
                switch (entry.Type)
                {
                    case LogType.Failure:
                    case LogType.Warning:
                        tdType.Attributes.Add("class", this.ErrorClass);
                        tdDescription.Attributes.Add("class", this.ErrorClass);
                        break;
                    case LogType.StartJob:
                    case LogType.EndJob:
                        tdType.Attributes.Add("class", this.HighlightClass);
                        tdDescription.Attributes.Add("class", this.HighlightClass);
                        break;
                    case LogType.Info:
                        tdType.Attributes.Add("class", this.NormalClass);
                        tdDescription.Attributes.Add("class", this.NormalClass);
                        break;
                }

                arrayTable.Rows.Add(tr);
                if (entry.Type == LogType.EndJob)
                {
                    var spaceTR = new HtmlTableRow();
                    spaceTR.Cells.Add(new HtmlTableCell { ColSpan = 2, InnerHtml = "&nbsp;" });
                    arrayTable.Rows.Add(spaceTR);
                }
            }

            return arrayTable;
        }

        public void ResetFlags()
        {
            this.valid = true;
        }

        /// <summary>The StartJob method adds a new LogEntry of type StartJob to the Logs collection.</summary>
        /// <param name="job">The description of the LogEntry.</param>
        public void StartJob(string job)
        {
            this.logs.Add(new LogEntry(LogType.StartJob, job));
            DnnLogger.Info(job);
        }
    }
}
