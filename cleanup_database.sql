-- Clean up database to keep only 10-12 projects
-- First, let's see what projects exist and keep the most recent ones

-- Delete old projects, keeping only the latest 12
DELETE FROM crc_webportal."Projects" 
WHERE "Id" NOT IN (
    SELECT "Id" 
    FROM crc_webportal."Projects" 
    ORDER BY "CreatedAt" DESC 
    LIMIT 12
);

-- Clean up orphaned data
DELETE FROM crc_webportal."Buildings" 
WHERE "ProjectId" NOT IN (SELECT "Id" FROM crc_webportal."Projects");

DELETE FROM crc_webportal."UnitTypes" 
WHERE "ProjectId" NOT IN (SELECT "Id" FROM crc_webportal."Projects");

DELETE FROM crc_webportal."NumberingPatterns" 
WHERE "ProjectId" NOT IN (SELECT "Id" FROM crc_webportal."Projects");

DELETE FROM crc_webportal."Units" 
WHERE "BuildingId" NOT IN (SELECT "Id" FROM crc_webportal."Buildings");

DELETE FROM crc_webportal."UnitOwnerships" 
WHERE "UnitId" NOT IN (SELECT "Id" FROM crc_webportal."Units");

-- Show remaining projects count
SELECT COUNT(*) as "RemainingProjects" FROM crc_webportal."Projects";
