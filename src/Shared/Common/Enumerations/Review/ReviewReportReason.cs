using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Enumerations.Review
{
	public enum ReviewReportReason
	{
		Spam = 1,              
		OffensiveLanguage = 2, 
		HateSpeech = 3,        
		FakeReview = 4,        
		IrrelevantContent = 5,
		Duplicate = 6,         
		Other = 99             
	}

}
