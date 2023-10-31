namespace XML_Handler
{
    public class Repo
    {
        public Parameters[] parameters = {
        new Parameters("EmployeeIncrements", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:PlanSchedule/dgc:Money"),
        new Parameters("EmployeeMaximum", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:BenefitMaximum"),
        new Parameters("SpouseIncrementsOrPercentage", "//docset:CriticalIllnessSpouse/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:PlanSchedule/dgc:Money"),
        new Parameters("SpouseMaximum", "//docset:CriticalIllnessSpouse/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:BenefitMaximum"),
        new Parameters("SpousePercOfEmployeeBenefitLimit", "//docset:CriticalIllnessSpouse/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:MaximumPercentageofEmployeeBenefit"),
        new Parameters("ChildBenefitAmount", "//docset:CriticalIllness-section/docset:CriticalIllness/docset:CriticalIllnessChildEligibility/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td/docset:BenefitMinimum"),
        new Parameters("ChildMaximum", "//docset:CriticalIllness-section/docset:CriticalIllness/docset:CriticalIllnessChildEligibility/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td/docset:BenefitMaximum"),
        new Parameters("ChildPercOfEmployeeBenefitLimit", "//docset:CriticalIllness-section/docset:CriticalIllness/docset:CriticalIllnessChildEligibility/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td/docset:MaximumPercentageofEmployeeBenefit"),
        new Parameters("EmployeeGuaranteeIssue", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:GuaranteeIssue"),
        new Parameters("SpouseGuaranteeIssue", "//docset:CriticalIllnessSpouse/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:GuaranteeIssue"),
        new Parameters("ChildGuaranteeIssue", "//docset:CriticalIllness-section/docset:CriticalIllness/docset:CriticalIllnessChildEligibility/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:GuaranteeIssue"),
        new Parameters("InvasiveOrMalignantCancer", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:CoveredConditions[text()='Cancer ']/../following-sibling::*/docset:BenefitPercentages"),
        new Parameters("Category1Vascular", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:CoveredConditions[text()='Heart Attack ']/../following-sibling::*/docset:BenefitPercentages"),
        new Parameters("OrganOrKidneyFailure", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:CoveredConditions/dgm:Condition[text()='organ failure ']/../../following-sibling::*/docset:BenefitPercentages"),
        new Parameters("RecurrenceDifferentIllnessWaitingPeriod", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:Recurrence"),
        new Parameters("SecondOccurrenceOfSameIllnessWaitingPeriod", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:AdditionalOccurrence"),
        new Parameters("AgeReduction", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:AgeReduction"),
        new Parameters("PreExistingCondition", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:Pre-ExistingConditionLimitation"),
        new Parameters("Portability", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:Portability/dgc:Number"),
        new Parameters("WellnessBenefit", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:WellnessBenefit-annualBenefitforCoveredWellnessExamsScreenings"),
        new Parameters("EmployerContributionLevel", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:EmployerContributionBenefitPercentages"),
        new Parameters("AgeBasis", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:PremiumRateBasis"),
        new Parameters("DependentAgeLimits", "//docset:DependentChildren/dgc:Number"),
        new Parameters("ParticipationRequirement", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:ParticipationRequirementBenefitPercentages"),
        new Parameters("RateAge25", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr[4]/xhtml:td[2]/docset:_10000"),
        new Parameters("RateAge35", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr[4]/xhtml:td[3]/docset:_10000"),
        new Parameters("RateAge45", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr[4]/xhtml:td[4]/docset:_10000"),
        new Parameters("RateAge55", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr[4]/xhtml:td[5]/docset:_10000"),
        new Parameters("IsWellnessBenefitCostIncluded", "//docset:CriticalIllness/xhtml:table/xhtml:tbody/xhtml:tr/xhtml:td//docset:EmployeeAddedtoMonthlyPremium"),
        new Parameters("RateGuarantee", "//docset:CriticalIllness-section//docset:CriticalIllness//docset:_1050-section//dg:chunk[2]//dgc:TimeDuration"),
        };
    }
}
