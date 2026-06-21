-- 1) retrieve all approved loans
SELECT
    l.Id,
    l.ApplicantName,
    l.LoanAmount,
    l.CreditScore,
    l.CreatedDate,
    u.Username AS CreatedBy
FROM  dbo.Loans l
JOIN  dbo.Users u ON u.Id = l.CreatedByUserId
WHERE l.Status = 1
ORDER BY l.CreatedDate DESC;


-- 2) average loan amount by status
SELECT
    CASE l.Status
        WHEN 0 THEN 'Pending'
        WHEN 1 THEN 'Approved'
        WHEN 2 THEN 'Rejected'
    END          AS StatusName,
    COUNT(*)     AS TotalLoans,
    AVG(l.LoanAmount) AS AverageLoanAmount
FROM  dbo.Loans l
GROUP BY l.Status
ORDER BY l.Status;


-- 3) identify high risk loans
-- definition: CreditScore < 650 AND LoanAmount > 30,000
SELECT
    l.Id,
    l.ApplicantName,
    l.LoanAmount,
    l.CreditScore,
    CASE l.Status
        WHEN 0 THEN 'Pending'
        WHEN 1 THEN 'Approved'
        WHEN 2 THEN 'Rejected'
    END       AS StatusName,
    u.Username AS CreatedBy
FROM  dbo.Loans l
JOIN  dbo.Users u ON u.Id = l.CreatedByUserId
WHERE l.CreditScore < 650
  AND l.LoanAmount  > 30000
ORDER BY l.LoanAmount DESC;