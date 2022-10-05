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

for(i in 1:nrow(my_data)){
  if(my_data$Application[i]=="Numpad"){
    my_data$Task[i] = my_data$Task[i] + 4
  }
  
  if(my_data$Application[i]=="Weather"){
    my_data$Task[i] = my_data$Task[i] + 6
  }
}

# Windows style: 
#my_data <- read.csv("C:\\Users\\tholl\\Dropbox\\Documents\\2022-AbbyMS\\ResponsiveARExperimentData.csv", header= TRUE, fileEncoding = 'UTF-8-BOM')

# make factors: 
my_data$Mode = factor(my_data$Mode)
my_data$Task = factor(my_data$Task)
my_data$userID = factor(my_data$userID)
my_data$Application = factor(my_data$Application)


# averaging over all the tasks per Task (since there is an unequal number of tasks it cannot easily be a separate within-subject variable)
f <- my_data

## TIME ##
# Some visualizations: bar plot with CI whiskers 
ggplot(my_data, aes(x=Task, y=Time)) + geom_bar(stat = "summary", fun = "mean", fill="lightsteelblue") + geom_errorbar(stat="summary", fun.data="mean_se", fun.args = list(mult = 1.96))
ggplot(my_data, aes(x=Mode, y=Time)) + geom_bar(stat = "summary", fun = "mean", fill="lightsteelblue") + geom_errorbar(stat="summary", fun.data="mean_se", fun.args = list(mult = 1.96))
# all data in box plots: 
ggboxplot(my_data, x = "Mode", y = "Time", color = "Task", palette = "jco")

res.aov <- anova_test( data = f, dv = Time, wid = userID, between = Mode, within = Task)
get_anova_table(res.aov)

pwc <- f %>% group_by(Task) %>% pairwise_t_test(Time ~ Mode, p.adjust.method = "bonferroni")
pwc

pwc <- f %>% group_by(Task) %>% pairwise_t_test(Time ~ Mode, p.adjust.method = "holm")
pwc

pwc <- f %>% group_by(Task) %>% wilcox_test(Time ~ Mode, paired=FALSE, exact=FALSE)
pwc

m <- art(data = f, Time ~ Mode * Task + Error(userID))
anova(m)

pairwise_comp <- contrast(emmeans(artlm(m,"Mode"), ~ Mode), method = "pairwise")
pairwise_comp

m <- art(data = f, Time ~ Mode * Task + (1 | userID))
anova(m)

shapiro.test(residuals(m))

pairwise_comp <- contrast(emmeans(artlm(m,"Mode"), ~ Mode), method = "pairwise")
pairwise_comp

library(lsmeans)
lsmeans(artlm(m,"Mode"), pairwise ~ Mode)

library(phia)
testInteractions(artlm(m,"Mode:Task"), pairwise=c("Mode", "Task"), adjustment = "holm")


## TIME Outliers Removed##
outliers <- f %>% group_by(Task, Mode) %>% identify_outliers(Time)
outliers

f_no_outliers <- anti_join (f, outliers)
ggboxplot(f_no_outliers, x = "Mode", y = "Time", color = "Task", palette = "jco")


res.aov <- anova_test( data = f_no_outliers, dv = Time, wid = userID, between = Mode, within = Task)
get_anova_table(res.aov)

pwc <- f_no_outliers %>% group_by(Task) %>% pairwise_t_test(Time ~ Mode, p.adjust.method = "bonferroni")
pwc

pwc <- f_no_outliers %>% group_by(Task) %>% pairwise_t_test(Time ~ Mode, p.adjust.method = "holm")
pwc

pwc <- f_no_outliers %>% group_by(Task) %>% wilcox_test(Time ~ Mode, paired=FALSE, exact=FALSE)
pwc

m <- art(data = f_no_outliers, Time ~ Mode * Task + Error(userID))
anova(m)

pairwise_comp <- contrast(emmeans(artlm(m,"Mode"), ~ Mode), method = "pairwise")
pairwise_comp

## # Guesses ##
# Some visualizations: bar plot with CI whiskers
ggplot(my_data, aes(x=Task, y=Guesses)) + geom_bar(stat = "summary", fun = "mean", fill="lightsteelblue") + geom_errorbar(stat="summary", fun.data="mean_se", fun.args = list(mult = 1.96))
ggplot(my_data, aes(x=Mode, y=Guesses)) + geom_bar(stat = "summary", fun = "mean", fill="lightsteelblue") + geom_errorbar(stat="summary", fun.data="mean_se", fun.args = list(mult = 1.96))
# all data in box plots:
ggboxplot(my_data, x = "Mode", y = "Guesses", color = "Task", palette = "jco")

res.aov <- anova_test( data = f, dv = Guesses, wid = userID, between = Mode, within = Task)
get_anova_table(res.aov)

pwc <- f %>% group_by(Task) %>% pairwise_t_test(Guesses ~ Mode, p.adjust.method = "bonferroni")
pwc

pwc <- f %>% group_by(Task) %>% pairwise_t_test(Guesses ~ Mode, p.adjust.method = "holm")
pwc

pwc <- f %>% group_by(Task) %>% wilcox_test(Guesses ~ Mode, paired=FALSE, exact=FALSE)
pwc


m <- art(data = f, Guesses ~ Mode * Task + Error(userID))
anova(m)

pairwise_comp <- contrast(emmeans(artlm(m,"Mode"), ~ Mode), method = "pairwise")
pairwise_comp

## # Guesses Outliers Removed##
outliers <- f %>% group_by(Task, Mode) %>% identify_outliers(Guesses)
outliers

f_no_outliers <- anti_join (f, outliers)
ggboxplot(f_no_outliers, x = "Mode", y = "Guesses", color = "Task", palette = "jco")


res.aov <- anova_test( data = f_no_outliers, dv = Guesses, wid = userID, between = Mode, within = Task)
get_anova_table(res.aov)

pwc <- f_no_outliers %>% group_by(Task) %>% pairwise_t_test(Guesses ~ Mode, p.adjust.method = "bonferroni")
pwc

pwc <- f_no_outliers %>% group_by(Task) %>% pairwise_t_test(Guesses ~ Mode, p.adjust.method = "holm")
pwc

m <- art(data = f_no_outliers, Guesses ~ Mode * Task + Error(userID))
anova(m)

pairwise_comp <- contrast(emmeans(artlm(m,"Mode"), ~ Mode), method = "pairwise")
pairwise_comp

## Avg.Dist ##
# Some visualizations: bar plot with CI whiskers
ggplot(my_data, aes(x=Task, y=Avg.Dist)) + geom_bar(stat = "summary", fun = "mean", fill="lightsteelblue") + geom_errorbar(stat="summary", fun.data="mean_se", fun.args = list(mult = 1.96))
ggplot(my_data, aes(x=Mode, y=Avg.Dist)) + geom_bar(stat = "summary", fun = "mean", fill="lightsteelblue") + geom_errorbar(stat="summary", fun.data="mean_se", fun.args = list(mult = 1.96))
# all data in box plots:
ggboxplot(my_data, x = "Mode", y = "Avg.Dist", color = "Task", palette = "jco")

res.aov <- anova_test( data = f, dv = Avg.Dist, wid = userID, between = Mode, within = Task)
get_anova_table(res.aov)

pwc <- f %>% group_by(Task) %>% pairwise_t_test(Avg.Dist ~ Mode, p.adjust.method = "bonferroni")
pwc

pwc <- f %>% group_by(Task) %>% pairwise_t_test(Avg.Dist ~ Mode, p.adjust.method = "holm")
pwc

pwc <- f %>% group_by(Task) %>% wilcox_test(Avg.Dist ~ Mode, paired=FALSE, exact=FALSE)
pwc

m <- art(data = f, Avg.Dist ~ Mode * Task + Error(userID))
anova(m)

pairwise_comp <- contrast(emmeans(artlm(m,"Mode"), ~ Mode), method = "pairwise")
pairwise_comp

## Avg Dist Outliers Removed##
outliers <- f %>% group_by(Task, Mode) %>% identify_outliers(Avg.Dist)
outliers

f_no_outliers <- anti_join (f, outliers)
ggboxplot(f_no_outliers, x = "Mode", y = "Avg.Dist", color = "Task", palette = "jco")


res.aov <- anova_test( data = f_no_outliers, dv = Avg.Dist, wid = userID, between = Mode, within = Task)
get_anova_table(res.aov)

pwc <- f_no_outliers %>% group_by(Task) %>% pairwise_t_test(Avg.Dist ~ Mode, p.adjust.method = "bonferroni")
pwc

pwc <- f_no_outliers %>% group_by(Task) %>% pairwise_t_test(Avg.Dist ~ Mode, p.adjust.method = "holm")
pwc

m <- art(data = f_no_outliers, Avg.Dist ~ Mode * Task + Error(userID))
anova(m)

pairwise_comp <- contrast(emmeans(artlm(m,"Mode"), ~ Mode), method = "pairwise")
pairwise_comp

## STD.Dist ##
# Some visualizations: bar plot with CI whiskers
ggplot(my_data, aes(x=Task, y=STD.Dist)) + geom_bar(stat = "summary", fun = "mean", fill="lightsteelblue") + geom_errorbar(stat="summary", fun.data="mean_se", fun.args = list(mult = 1.96))
ggplot(my_data, aes(x=Mode, y=STD.Dist)) + geom_bar(stat = "summary", fun = "mean", fill="lightsteelblue") + geom_errorbar(stat="summary", fun.data="mean_se", fun.args = list(mult = 1.96))
# all data in box plots:
ggboxplot(my_data, x = "Mode", y = "STD.Dist", color = "Task", palette = "jco")
# aggregated over task:
ggboxplot(f, x = "Mode", y = "STD.Dist", color = "Task", palette = "jco")


res.aov <- anova_test( data = f, dv = STD.Dist, wid = userID, between = Mode, within = Task)
get_anova_table(res.aov)

pwc <- f %>% group_by(Task) %>% pairwise_t_test(STD.Dist ~ Mode, p.adjust.method = "bonferroni")
pwc

pwc <- f %>% group_by(Task) %>% pairwise_t_test(STD.Dist ~ Mode, p.adjust.method = "holm")
pwc

m <- art(data = f, STD.Dist ~ Mode * Task + Error(userID))
anova(m)

pairwise_comp <- contrast(emmeans(artlm(m,"Mode"), ~ Mode), method = "pairwise")
pairwise_comp

## Std Dist Outliers Removed##
outliers <- f %>% group_by(Task, Mode) %>% identify_outliers(STD.Dist)
outliers

f_no_outliers <- anti_join (f, outliers)
ggboxplot(f_no_outliers, x = "Mode", y = "STD.Dist", color = "Task", palette = "jco")


res.aov <- anova_test( data = f_no_outliers, dv = STD.Dist, wid = userID, between = Mode, within = Task)
get_anova_table(res.aov)

pwc <- f_no_outliers %>% group_by(Task) %>% pairwise_t_test(STD.Dist ~ Mode, p.adjust.method = "bonferroni")
pwc

pwc <- f_no_outliers %>% group_by(Task) %>% pairwise_t_test(STD.Dist ~ Mode, p.adjust.method = "holm")
pwc

m <- art(data = f_no_outliers, STD.Dist ~ Mode * Task + Error(userID))
anova(m)

pairwise_comp <- contrast(emmeans(artlm(m,"Mode"), ~ Mode), method = "pairwise")
pairwise_comp
