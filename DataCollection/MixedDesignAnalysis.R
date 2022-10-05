rm(list = ls())

#library(FSA) # <?>
#library(DescTools)
#library(tidyr)
#library(tidyverse)

library(plyr)
library(dplyr)
library(ggplot2)
library(ggpubr)
library(rstatix)
library(emmeans)
library(phia)       #testInteractions
library(tidyr)      #spread
library(ARTool)     #art, artlm

# Unix style: 
my_data <- read.csv("ExperimentData.csv", header = TRUE, fileEncoding = 'UTF-8-BOM')

# Windows style: 
#my_data <- read.csv("C:\\Users\\tholl\\Dropbox\\Documents\\2022-AbbyMS\\ResponsiveARExperimentData.csv", header= TRUE, fileEncoding = 'UTF-8-BOM')

# make factors: 
my_data$Mode = factor(my_data$Mode)
my_data$Application = factor(my_data$Application)
my_data$userID = factor(my_data$userID)
my_data$Task = factor(my_data$Task)


# averaging over all the tasks per application (since there is an unequal number of tasks it cannot easily be a separate within-subject variable)
#f <- ddply(select(my_data, -Task),.(userID, Mode, Application),colwise(mean))
f <- ddply(select(my_data, -Task),.(userID, Mode, Application),colwise(sum))

## TIME ##
# Some visualizations: bar plot with CI whiskers 
ggplot(my_data, aes(x=Application, y=Time)) + geom_bar(stat = "summary", fun = "mean", fill="lightsteelblue") + geom_errorbar(stat="summary", fun.data="mean_se", fun.args = list(mult = 1.96))
ggplot(my_data, aes(x=Mode, y=Time)) + geom_bar(stat = "summary", fun = "mean", fill="lightsteelblue") + geom_errorbar(stat="summary", fun.data="mean_se", fun.args = list(mult = 1.96))
# all data in box plots: 
ggboxplot(my_data, x = "Mode", y = "Time", color = "Application", palette = "jco")
# aggregated over task: 
ggboxplot(f, x = "Mode", y = "Time", color = "Application", palette = "jco")


res.aov <- anova_test( data = f, dv = Time, wid = userID, between = Mode, within = Application)
get_anova_table(res.aov)

pwc <- f %>% group_by(Application) %>% pairwise_t_test(Time ~ Mode, p.adjust.method = "bonferroni")
pwc

pwc <- f %>% group_by(Application) %>% pairwise_t_test(Time ~ Mode, p.adjust.method = "holm")
pwc


m <- art(data = f, Time ~ Mode * Application + Error(userID))
anova(m)

pairwise_comp <- contrast(emmeans(artlm(m,"Mode"), ~ Mode), method = "pairwise")
pairwise_comp


## TIME Outliers Removed##
outliers <- f %>% group_by(Application, Mode) %>% identify_outliers(Time)
outliers

f_no_outliers <- anti_join (f, outliers)
ggboxplot(f_no_outliers, x = "Mode", y = "Time", color = "Application", palette = "jco")


res.aov <- anova_test( data = f_no_outliers, dv = Time, wid = userID, between = Mode, within = Application)
get_anova_table(res.aov)

pwc <- f_no_outliers %>% group_by(Application) %>% pairwise_t_test(Time ~ Mode, p.adjust.method = "bonferroni")
pwc

pwc <- f_no_outliers %>% group_by(Application) %>% pairwise_t_test(Time ~ Mode, p.adjust.method = "holm")
pwc

m <- art(data = f_no_outliers, Time ~ Mode * Application + Error(userID))
anova(m)

pairwise_comp <- contrast(emmeans(artlm(m,"Mode"), ~ Mode), method = "pairwise")
pairwise_comp

## # Guesses ##
# Some visualizations: bar plot with CI whiskers 
ggplot(my_data, aes(x=Application, y=Guesses)) + geom_bar(stat = "summary", fun = "mean", fill="lightsteelblue") + geom_errorbar(stat="summary", fun.data="mean_se", fun.args = list(mult = 1.96))
ggplot(my_data, aes(x=Mode, y=Guesses)) + geom_bar(stat = "summary", fun = "mean", fill="lightsteelblue") + geom_errorbar(stat="summary", fun.data="mean_se", fun.args = list(mult = 1.96))
# all data in box plots: 
ggboxplot(my_data, x = "Mode", y = "Guesses", color = "Application", palette = "jco")
# aggregated over task: 
ggboxplot(f, x = "Mode", y = "Guesses", color = "Application", palette = "jco")


res.aov <- anova_test( data = f, dv = Guesses, wid = userID, between = Mode, within = Application)
get_anova_table(res.aov)

pwc <- f %>% group_by(Application) %>% pairwise_t_test(Guesses ~ Mode, p.adjust.method = "bonferroni")
pwc

pwc <- f %>% group_by(Application) %>% pairwise_t_test(Guesses ~ Mode, p.adjust.method = "holm")
pwc

m <- art(data = f, Guesses ~ Mode * Application + Error(userID))
anova(m)

pairwise_comp <- contrast(emmeans(artlm(m,"Mode"), ~ Mode), method = "pairwise")
pairwise_comp

## # Guesses Outliers Removed##
outliers <- f %>% group_by(Application, Mode) %>% identify_outliers(Guesses)
outliers

f_no_outliers <- anti_join (f, outliers)
ggboxplot(f_no_outliers, x = "Mode", y = "Guesses", color = "Application", palette = "jco")


res.aov <- anova_test( data = f_no_outliers, dv = Guesses, wid = userID, between = Mode, within = Application)
get_anova_table(res.aov)

pwc <- f_no_outliers %>% group_by(Application) %>% pairwise_t_test(Guesses ~ Mode, p.adjust.method = "bonferroni")
pwc

pwc <- f_no_outliers %>% group_by(Application) %>% pairwise_t_test(Guesses ~ Mode, p.adjust.method = "holm")
pwc

m <- art(data = f_no_outliers, Guesses ~ Mode * Application + Error(userID))
anova(m)

pairwise_comp <- contrast(emmeans(artlm(m,"Mode"), ~ Mode), method = "pairwise")
pairwise_comp


f <- ddply(select(my_data, -Task),.(userID, Mode, Application),colwise(mean))
## Avg.Dist ##
# Some visualizations: bar plot with CI whiskers 
ggplot(my_data, aes(x=Application, y=Avg.Dist)) + geom_bar(stat = "summary", fun = "mean", fill="lightsteelblue") + geom_errorbar(stat="summary", fun.data="mean_se", fun.args = list(mult = 1.96))
ggplot(my_data, aes(x=Mode, y=Avg.Dist)) + geom_bar(stat = "summary", fun = "mean", fill="lightsteelblue") + geom_errorbar(stat="summary", fun.data="mean_se", fun.args = list(mult = 1.96))
# all data in box plots: 
ggboxplot(my_data, x = "Mode", y = "Avg.Dist", color = "Application", palette = "jco")
# aggregated over task: 
ggboxplot(f, x = "Mode", y = "Avg.Dist", color = "Application", palette = "jco")


res.aov <- anova_test( data = f, dv = Avg.Dist, wid = userID, between = Mode, within = Application)
get_anova_table(res.aov)

pwc <- f %>% group_by(Application) %>% pairwise_t_test(Avg.Dist ~ Mode, p.adjust.method = "bonferroni")
pwc

pwc <- f %>% group_by(Application) %>% pairwise_t_test(Avg.Dist ~ Mode, p.adjust.method = "holm")
pwc

m <- art(data = f, Avg.Dist ~ Mode * Application + Error(userID))
anova(m)

pairwise_comp <- contrast(emmeans(artlm(m,"Mode"), ~ Mode), method = "pairwise")
pairwise_comp

## Avg Dist Outliers Removed##
outliers <- f %>% group_by(Application, Mode) %>% identify_outliers(Avg.Dist)
outliers

f_no_outliers <- anti_join (f, outliers)
ggboxplot(f_no_outliers, x = "Mode", y = "Avg.Dist", color = "Application", palette = "jco")


res.aov <- anova_test( data = f_no_outliers, dv = Avg.Dist, wid = userID, between = Mode, within = Application)
get_anova_table(res.aov)

pwc <- f_no_outliers %>% group_by(Application) %>% pairwise_t_test(Avg.Dist ~ Mode, p.adjust.method = "bonferroni")
pwc

pwc <- f_no_outliers %>% group_by(Application) %>% pairwise_t_test(Avg.Dist ~ Mode, p.adjust.method = "holm")
pwc

m <- art(data = f_no_outliers, Avg.Dist ~ Mode * Application + Error(userID))
anova(m)

pairwise_comp <- contrast(emmeans(artlm(m,"Mode"), ~ Mode), method = "pairwise")
pairwise_comp

## STD.Dist ##
# Some visualizations: bar plot with CI whiskers 
ggplot(my_data, aes(x=Application, y=STD.Dist)) + geom_bar(stat = "summary", fun = "mean", fill="lightsteelblue") + geom_errorbar(stat="summary", fun.data="mean_se", fun.args = list(mult = 1.96))
ggplot(my_data, aes(x=Mode, y=STD.Dist)) + geom_bar(stat = "summary", fun = "mean", fill="lightsteelblue") + geom_errorbar(stat="summary", fun.data="mean_se", fun.args = list(mult = 1.96))
# all data in box plots: 
ggboxplot(my_data, x = "Mode", y = "STD.Dist", color = "Application", palette = "jco")
# aggregated over task: 
ggboxplot(f, x = "Mode", y = "STD.Dist", color = "Application", palette = "jco")


res.aov <- anova_test( data = f, dv = STD.Dist, wid = userID, between = Mode, within = Application)
get_anova_table(res.aov)

pwc <- f %>% group_by(Application) %>% pairwise_t_test(STD.Dist ~ Mode, p.adjust.method = "bonferroni")
pwc

pwc <- f %>% group_by(Application) %>% pairwise_t_test(STD.Dist ~ Mode, p.adjust.method = "holm")
pwc

m <- art(data = f, STD.Dist ~ Mode * Application + Error(userID))
anova(m)

pairwise_comp <- contrast(emmeans(artlm(m,"Mode"), ~ Mode), method = "pairwise")
pairwise_comp

## Std Dist Outliers Removed##
outliers <- f %>% group_by(Application, Mode) %>% identify_outliers(STD.Dist)
outliers

f_no_outliers <- anti_join (f, outliers)
ggboxplot(f_no_outliers, x = "Mode", y = "STD.Dist", color = "Application", palette = "jco")


res.aov <- anova_test( data = f_no_outliers, dv = STD.Dist, wid = userID, between = Mode, within = Application)
get_anova_table(res.aov)

pwc <- f_no_outliers %>% group_by(Application) %>% pairwise_t_test(STD.Dist ~ Mode, p.adjust.method = "bonferroni")
pwc

pwc <- f_no_outliers %>% group_by(Application) %>% pairwise_t_test(STD.Dist ~ Mode, p.adjust.method = "holm")
pwc

m <- art(data = f_no_outliers, STD.Dist ~ Mode * Application + Error(userID))
anova(m)

pairwise_comp <- contrast(emmeans(artlm(m,"Mode"), ~ Mode), method = "pairwise")
pairwise_comp

#
#
#
#
