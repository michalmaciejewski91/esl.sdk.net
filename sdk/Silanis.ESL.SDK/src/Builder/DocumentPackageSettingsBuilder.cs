using System;
using System.Collections.Generic;
using Silanis.ESL.SDK.Internal;
using Silanis.ESL.API;

namespace Silanis.ESL.SDK
{
	public class DocumentPackageSettingsBuilder
	{
		private Nullable<bool> enableInPerson = null;
		private Nullable<bool> enableOptOut = null;
		private Nullable<bool> enableDecline = null;
		private Nullable<bool> hideWatermark = null;
		private Nullable<bool> hideCaptureText = null;
		private List<string> optOutReasons = new List<string>();
		private Nullable<int> maxAuthAttempts = null;
		private Nullable<bool> showDownloadButton = true;
		private Nullable<bool> showDialogOnComplete = null;

		private string linkText = null;
		private string linkTooltip = null;
		private string linkHref = null;

		private CeremonyLayoutSettings ceremonyLayoutSettings = null;

		private DocumentPackageSettingsBuilder ()
		{
		}

        public DocumentPackageSettingsBuilder WithDialogOnComplete()
        {
            showDialogOnComplete = true;
            return this;
        }

		public DocumentPackageSettingsBuilder WithoutDialogOnComplete()
		{
			showDialogOnComplete = false;
			return this;
		}

		public DocumentPackageSettingsBuilder WithInPerson()
		{
			enableInPerson = true;
			return this;
		}

		public DocumentPackageSettingsBuilder WithoutInPerson()
		{
			enableInPerson = false;
			return this;
		}

		public DocumentPackageSettingsBuilder WithOptOut()
		{
			enableOptOut = true;
			return this;
		}

		public DocumentPackageSettingsBuilder WithoutOptOut()
		{
			enableOptOut = false;
			return this;
		}

		public DocumentPackageSettingsBuilder WithDecline()
		{
			enableDecline = true;
			return this;
		}

		public DocumentPackageSettingsBuilder WithoutDecline() {
			enableDecline = false;
			return this;
		}

        public DocumentPackageSettingsBuilder WithWatermark() {
            hideWatermark = false;
            return this;
        }

		public DocumentPackageSettingsBuilder WithoutWatermark() {
			hideWatermark = true;
			return this;
		}

        public DocumentPackageSettingsBuilder WithCaptureText() {
            hideCaptureText = false;
            return this;
        }

		public DocumentPackageSettingsBuilder WithoutCaptureText() {
			hideCaptureText = true;
			return this;
		}

		public DocumentPackageSettingsBuilder WithHandOverLinkHref( String href )
		{
            Asserts.NotEmptyOrNull(href, "href");

			linkHref = href;

            //If no protocol was specified, we assume https
            if (!linkHref.StartsWith("http://") && !linkHref.StartsWith("https://"))
            {
                linkHref = "https://" + linkHref;
            }

			return this;
		}

		public DocumentPackageSettingsBuilder WithHandOverLinkText( String text )
		{
			linkText = text;
			return this;
		}

		public DocumentPackageSettingsBuilder WithHandOverLinkTooltip( String tooltip )
		{
			linkTooltip = tooltip;
			return this;
		}

		public DocumentPackageSettingsBuilder WithOptOutReason( String reason )
		{
			optOutReasons.Add( reason );
			return this;
		}

		public DocumentPackageSettingsBuilder WithCeremonyLayoutSettings( CeremonyLayoutSettingsBuilder builder )
		{
			return WithCeremonyLayoutSettings (builder.Build ());
		}

		public DocumentPackageSettingsBuilder WithCeremonyLayoutSettings( CeremonyLayoutSettings ceremonyLayoutSettings )
		{
			this.ceremonyLayoutSettings = ceremonyLayoutSettings;
			return this;
		}

		public DocumentPackageSettings build()
		{
			DocumentPackageSettings result = new DocumentPackageSettings ();

			result.EnableInPerson = enableInPerson;
			result.EnableOptOut = enableOptOut;
			result.EnableDecline = enableDecline;
			result.HideWatermark = hideWatermark;
			result.HideCaptureText = hideCaptureText;
			foreach ( string reason in optOutReasons )
				result.OptOutReasons.Add( reason );
			result.MaxAuthAttempts = maxAuthAttempts;
			result.ShowDownloadButton = showDownloadButton;
			result.ShowDialogOnComplete = showDialogOnComplete;
			result.LinkHref = linkHref;
			result.LinkText = linkText;
			result.LinkTooltip = linkTooltip;

			result.CeremonyLayoutSettings = ceremonyLayoutSettings;

			return result;
		}

        public static DocumentPackageSettingsBuilder NewDocumentPackageSettings()
        {
            return new DocumentPackageSettingsBuilder();
        }

        internal DocumentPackageSettingsBuilder( PackageSettings apiPackageSettings )
        {
            enableInPerson = apiPackageSettings.Ceremony.InPerson;
            enableOptOut = apiPackageSettings.Ceremony.OptOutButton;
            enableDecline = apiPackageSettings.Ceremony.DeclineButton;
            hideWatermark = apiPackageSettings.Ceremony.HideWatermark;
            hideCaptureText = apiPackageSettings.Ceremony.HideCaptureText;

            foreach (string reason in apiPackageSettings.Ceremony.OptOutReasons)
            {
                optOutReasons.Add(reason);
            }

            maxAuthAttempts = apiPackageSettings.Ceremony.MaxAuthFailsAllowed;

            if (apiPackageSettings.Ceremony.DocumentToolbarOptions != null)
            {
                showDownloadButton = apiPackageSettings.Ceremony.DocumentToolbarOptions.DownloadButton;
            }

            if (apiPackageSettings.Ceremony.Events != null && apiPackageSettings.Ceremony.Events.Complete != null)
            {
                showDialogOnComplete = apiPackageSettings.Ceremony.Events.Complete.Dialog;
            }

            if (apiPackageSettings.Ceremony.HandOver != null)
            {
                linkHref = apiPackageSettings.Ceremony.HandOver.Href;
                linkText = apiPackageSettings.Ceremony.HandOver.Text;
                linkTooltip = apiPackageSettings.Ceremony.HandOver.Title;
            }
        }

	}
}
